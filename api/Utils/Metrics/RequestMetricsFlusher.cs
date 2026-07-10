using api.Models;
using Microsoft.EntityFrameworkCore;

namespace api.Utils.Metrics
{
    /// <summary>
    /// Background service that drains the in-memory <see cref="RequestMetricsCollector"/>
    /// once per hour and persists an aggregate row to Postgres. Exactly one DB
    /// write per hour regardless of traffic volume, so it is safe at high RPS.
    /// The last partial hour is also flushed on graceful shutdown.
    /// </summary>
    public class RequestMetricsFlusher : BackgroundService
    {
        private static readonly TimeSpan FlushInterval = TimeSpan.FromHours(1);

        private readonly RequestMetricsCollector _collector;
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly ILogger<RequestMetricsFlusher> _logger;

        public RequestMetricsFlusher(
            RequestMetricsCollector collector,
            IServiceScopeFactory scopeFactory,
            ILogger<RequestMetricsFlusher> logger)
        {
            _collector = collector;
            _scopeFactory = scopeFactory;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            using var timer = new PeriodicTimer(FlushInterval);
            try
            {
                while (await timer.WaitForNextTickAsync(stoppingToken))
                {
                    await FlushAsync(stoppingToken);
                }
            }
            catch (OperationCanceledException)
            {
                // Shutting down.
            }
        }

        public override async Task StopAsync(CancellationToken cancellationToken)
        {
            // Persist whatever accumulated in the final partial hour.
            await FlushAsync(cancellationToken);
            await base.StopAsync(cancellationToken);
        }

        private async Task FlushAsync(CancellationToken cancellationToken)
        {
            var aggregate = _collector.DrainCurrentHour();
            if (aggregate is null)
            {
                return;
            }

            var bucket = TruncateToHour(DateTime.UtcNow);

            try
            {
                using var scope = _scopeFactory.CreateScope();
                var db = scope.ServiceProvider.GetRequiredService<DBContext>();

                var existing = await db.RequestMetricRollups
                    .FirstOrDefaultAsync(r => r.HourBucketUtc == bucket, cancellationToken);

                if (existing is null)
                {
                    db.RequestMetricRollups.Add(new RequestMetricRollup
                    {
                        HourBucketUtc = bucket,
                        RequestCount = aggregate.RequestCount,
                        TotalResponseBytes = aggregate.TotalResponseBytes,
                        MaxResponseBytes = aggregate.MaxResponseBytes,
                        LargestRequestPath = aggregate.LargestRequestPath,
                    });
                    await db.SaveChangesAsync(cancellationToken);
                }
                else
                {
                    // Merge into the existing bucket (e.g. after a restart mid-hour).
                    var mergedMaxBytes = Math.Max(existing.MaxResponseBytes, aggregate.MaxResponseBytes);
                    var mergedMaxPath = aggregate.MaxResponseBytes > existing.MaxResponseBytes
                        ? aggregate.LargestRequestPath
                        : existing.LargestRequestPath;

                    await db.RequestMetricRollups
                        .Where(r => r.Id == existing.Id)
                        .ExecuteUpdateAsync(s => s
                            .SetProperty(r => r.RequestCount, r => r.RequestCount + aggregate.RequestCount)
                            .SetProperty(r => r.TotalResponseBytes, r => r.TotalResponseBytes + aggregate.TotalResponseBytes)
                            .SetProperty(r => r.MaxResponseBytes, mergedMaxBytes)
                            .SetProperty(r => r.LargestRequestPath, mergedMaxPath),
                            cancellationToken);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to persist request metrics rollup for bucket {Bucket}.", bucket);
            }
        }

        private static DateTime TruncateToHour(DateTime value) =>
            new(value.Year, value.Month, value.Day, value.Hour, 0, 0, DateTimeKind.Utc);
    }
}
