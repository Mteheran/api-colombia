using api.Utils;
using api.Utils.Metrics;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.Annotations;
using MetricsEndpointMetadataMessages = api.Utils.Messages.EndpointMetadata.MetricsEndpoint;

namespace api.Routes
{
    public static class MetricsRoutes
    {
        public static void RegisterMetricsAPI(WebApplication app)
        {
            const string API_METRICS_ROUTE_COMPLETE = $"{Util.API_ROUTE}{Util.API_VERSION}{Util.METRICS_ROUTE}";
            const string API_METRICS_TAG = "Metrics";

            app.MapGet(API_METRICS_ROUTE_COMPLETE, async (DBContext db, RequestMetricsCollector collector) =>
            {
                var recent = collector.GetRecent()
                    .Select(r => new RecentRequestDto(r.TimestampUtc, r.Method, r.Path, r.StatusCode, r.ResponseBytes))
                    .ToList();

                var currentHour = collector.PeekCurrentHour();

                // Largest response ever recorded: the max of the persisted rollups
                // and the current (not-yet-flushed) hour still in memory.
                var largestRollup = await db.RequestMetricRollups
                    .OrderByDescending(r => r.MaxResponseBytes)
                    .Select(r => new { r.MaxResponseBytes, r.LargestRequestPath })
                    .FirstOrDefaultAsync();

                long largestBytes = currentHour.MaxResponseBytes;
                string? largestPath = currentHour.LargestRequestPath;
                if (largestRollup is not null && largestRollup.MaxResponseBytes > largestBytes)
                {
                    largestBytes = largestRollup.MaxResponseBytes;
                    largestPath = largestRollup.LargestRequestPath;
                }

                var monthly = await db.RequestMetricRollups
                    .GroupBy(r => new { r.HourBucketUtc.Year, r.HourBucketUtc.Month })
                    .OrderByDescending(g => g.Key.Year).ThenByDescending(g => g.Key.Month)
                    .Select(g => new MonthlyUsageDto(
                        g.Key.Year,
                        g.Key.Month,
                        g.Sum(r => r.RequestCount),
                        g.Sum(r => r.TotalResponseBytes)))
                    .ToListAsync();

                var response = new MetricsResponseDto(
                    GeneratedAtUtc: DateTime.UtcNow,
                    CurrentHourRequestCount: currentHour.RequestCount,
                    CurrentHourResponseBytes: currentHour.TotalResponseBytes,
                    LargestResponseBytes: largestBytes,
                    LargestResponsePath: largestPath,
                    RecentRequests: recent,
                    MonthlyUsage: monthly);

                return Results.Ok(response);
            })
            .WithTags(API_METRICS_TAG)
            .Produces<MetricsResponseDto>(200)
            // Short cache so the public endpoint can't be hammered while staying fresh.
            .CacheOutput(policy => policy.Expire(TimeSpan.FromSeconds(60)))
            .WithMetadata(new SwaggerOperationAttribute(
                summary: MetricsEndpointMetadataMessages.MESSAGE_METRICS_SUMMARY,
                description: MetricsEndpointMetadataMessages.MESSAGE_METRICS_DESCRIPTION));
        }
    }

    public record RecentRequestDto(DateTime TimestampUtc, string Method, string Path, int StatusCode, long ResponseBytes);

    public record MonthlyUsageDto(int Year, int Month, long RequestCount, long TotalResponseBytes);

    public record MetricsResponseDto(
        DateTime GeneratedAtUtc,
        long CurrentHourRequestCount,
        long CurrentHourResponseBytes,
        long LargestResponseBytes,
        string? LargestResponsePath,
        IReadOnlyList<RecentRequestDto> RecentRequests,
        IReadOnlyList<MonthlyUsageDto> MonthlyUsage);
}
