using api.Models;
using api.Utils.Metrics;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace api.Tests.Unit;

/// <summary>
/// The flusher persists the hourly rollup. Its merge branch — a bucket that already exists after a
/// restart mid-hour — uses <c>ExecuteUpdateAsync</c>, which the in-memory provider cannot run, so it
/// had no coverage at all. These tests run against an in-memory SQLite database instead, which is
/// relational and executes the real SQL update.
///
/// <see cref="RequestMetricsFlusher.StopAsync"/> is the entry point: it flushes the final partial
/// hour on shutdown, so calling it exercises exactly the code the hourly timer runs.
/// </summary>
public sealed class RequestMetricsFlusherTests : IDisposable
{
    private readonly SqliteConnection _connection = new("DataSource=:memory:");
    private readonly ServiceProvider _services;
    private readonly RequestMetricsCollector _collector = new();
    private readonly CapturingLogger _logger = new();

    public RequestMetricsFlusherTests()
    {
        _connection.Open();
        _services = new ServiceCollection()
            .AddDbContext<DBContext>(o => o.UseSqlite(_connection))
            .BuildServiceProvider();
    }

    public void Dispose()
    {
        _services.Dispose();
        _connection.Dispose();
    }

    private void CreateSchema()
    {
        using var scope = _services.CreateScope();
        scope.ServiceProvider.GetRequiredService<DBContext>().Database.EnsureCreated();
    }

    private async Task FlushAsync()
    {
        var flusher = new RequestMetricsFlusher(_collector, _services.GetRequiredService<IServiceScopeFactory>(), _logger);
        await flusher.StopAsync(CancellationToken.None);
    }

    private List<RequestMetricRollup> Rollups()
    {
        using var scope = _services.CreateScope();
        return [.. scope.ServiceProvider.GetRequiredService<DBContext>().RequestMetricRollups.OrderBy(r => r.HourBucketUtc)];
    }

    private void SeedRollups(params RequestMetricRollup[] rollups)
    {
        using var scope = _services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<DBContext>();
        db.RequestMetricRollups.AddRange(rollups);
        db.SaveChanges();
    }

    private static DateTime CurrentHour()
    {
        var now = DateTime.UtcNow;
        return new DateTime(now.Year, now.Month, now.Day, now.Hour, 0, 0, DateTimeKind.Utc);
    }

    /// <summary>
    /// Seeds the same rollup for this hour and the next one, so the test cannot flake if the clock
    /// crosses an hour boundary between seeding and flushing — whichever bucket the flusher computes,
    /// exactly one existing row is merged into.
    /// </summary>
    private void SeedExistingBucket(long maxBytes, string path)
    {
        var hour = CurrentHour();
        SeedRollups(
            new RequestMetricRollup { HourBucketUtc = hour, RequestCount = 10, TotalResponseBytes = 1_000, MaxResponseBytes = maxBytes, LargestRequestPath = path },
            new RequestMetricRollup { HourBucketUtc = hour.AddHours(1), RequestCount = 10, TotalResponseBytes = 1_000, MaxResponseBytes = maxBytes, LargestRequestPath = path });
    }

    private RequestMetricRollup MergedRow() => Assert.Single(Rollups(), r => r.RequestCount != 10);

    private void Record(string path, long bytes) =>
        _collector.Record(new RequestSample(DateTime.UtcNow, "GET", path, 200, bytes));

    [Fact]
    public async Task WithNoTraffic_WritesNothing()
    {
        CreateSchema();

        await FlushAsync();

        Assert.Empty(Rollups());
        Assert.Empty(_logger.Errors);
    }

    [Fact]
    public async Task WithTraffic_InsertsOneRollupForTheCurrentHour()
    {
        CreateSchema();
        Record("/api/v1/City", 100);
        Record("/api/v1/Department", 300);

        var before = CurrentHour();
        await FlushAsync();

        var rollup = Assert.Single(Rollups());
        Assert.InRange(rollup.HourBucketUtc, before, before.AddHours(1));
        Assert.Equal(0, rollup.HourBucketUtc.Minute);
        Assert.Equal(2, rollup.RequestCount);
        Assert.Equal(400, rollup.TotalResponseBytes);
        Assert.Equal(300, rollup.MaxResponseBytes);
        Assert.Equal("/api/v1/Department", rollup.LargestRequestPath);
    }

    [Fact]
    public async Task Flushing_DrainsTheCollector()
    {
        CreateSchema();
        Record("/a", 1);

        await FlushAsync();

        Assert.Null(_collector.DrainCurrentHour());
    }

    [Fact]
    public async Task ExistingBucket_WithASmallerMax_IsMergedAndTakesTheNewLargestPath()
    {
        CreateSchema();
        SeedExistingBucket(maxBytes: 50, path: "/old");
        Record("/new", 500);

        await FlushAsync();

        Assert.Equal(2, Rollups().Count);
        var merged = MergedRow();
        Assert.Equal(11, merged.RequestCount);
        Assert.Equal(1_500, merged.TotalResponseBytes);
        Assert.Equal(500, merged.MaxResponseBytes);
        Assert.Equal("/new", merged.LargestRequestPath);
    }

    [Fact]
    public async Task ExistingBucket_WithALargerMax_KeepsItsLargestPath()
    {
        CreateSchema();
        SeedExistingBucket(maxBytes: 9_000, path: "/old");
        Record("/new", 500);

        await FlushAsync();

        var merged = MergedRow();
        Assert.Equal(11, merged.RequestCount);
        Assert.Equal(9_000, merged.MaxResponseBytes);
        Assert.Equal("/old", merged.LargestRequestPath);
    }

    [Fact]
    public async Task WhenTheDatabaseFails_LogsTheErrorInsteadOfThrowing()
    {
        // No schema: the query for the existing bucket fails with "no such table".
        Record("/a", 1);

        await FlushAsync();

        var error = Assert.Single(_logger.Errors);
        Assert.NotNull(error.Exception);
    }

    [Fact]
    public async Task ExecuteAsync_StopsCleanlyWhenCancelled()
    {
        var flusher = new RequestMetricsFlusher(_collector, _services.GetRequiredService<IServiceScopeFactory>(), _logger);
        using var cts = new CancellationTokenSource();

        await flusher.StartAsync(cts.Token);
        await cts.CancelAsync();
        await flusher.StopAsync(CancellationToken.None);

        Assert.Empty(_logger.Errors);
    }

    private sealed class CapturingLogger : ILogger<RequestMetricsFlusher>
    {
        public List<(LogLevel Level, Exception? Exception)> Entries { get; } = [];

        public IEnumerable<(LogLevel Level, Exception? Exception)> Errors =>
            Entries.Where(e => e.Level >= LogLevel.Error);

        public IDisposable? BeginScope<TState>(TState state) where TState : notnull => null;

        public bool IsEnabled(LogLevel logLevel) => true;

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception,
            Func<TState, Exception?, string> formatter) => Entries.Add((logLevel, exception));
    }
}
