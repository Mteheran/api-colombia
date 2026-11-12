using System.Text.Json;
using api.Models;
using api.Tests.Infrastructure;
using Microsoft.Extensions.DependencyInjection;

namespace api.Tests.Integration.Platform;

/// <summary>
/// The metrics endpoint merges the in-memory current hour with the persisted hourly rollups. The
/// existing metrics test runs against an empty rollup table, so the "largest response ever came
/// from a past hour" branch and the monthly grouping were never exercised with data.
///
/// One test per class on purpose: the endpoint is output-cached for 60 seconds, so a second call on
/// the same host would read the first one's response.
/// </summary>
public class MetricsRollupIntegrationTests : IClassFixture<IsolatedFactory>
{
    private const long PersistedMax = 50_000_000;
    private const string PersistedPath = "/api/v1/persisted/largest";

    private readonly HttpClient _client;

    public MetricsRollupIntegrationTests(IsolatedFactory factory)
    {
        factory.EnsureSeeded();

        using (var scope = factory.Services.CreateScope())
        {
            var db = scope.ServiceProvider.GetRequiredService<DBContext>();
            if (!db.RequestMetricRollups.Any())
            {
                db.RequestMetricRollups.AddRange(
                    Rollup(new DateTime(2026, 9, 1, 10, 0, 0, DateTimeKind.Utc), 100, 1_000, 10),
                    Rollup(new DateTime(2026, 9, 2, 11, 0, 0, DateTimeKind.Utc), 50, 500, PersistedMax, PersistedPath),
                    Rollup(new DateTime(2026, 10, 5, 8, 0, 0, DateTimeKind.Utc), 7, 70, 20));
                db.SaveChanges();
            }
        }

        _client = factory.CreateClient();
    }

    private static RequestMetricRollup Rollup(DateTime hour, long count, long bytes, long max, string? path = "/x") =>
        new() { HourBucketUtc = hour, RequestCount = count, TotalResponseBytes = bytes, MaxResponseBytes = max, LargestRequestPath = path };

    [Fact]
    public async Task Metrics_CombinesPersistedRollupsWithTheCurrentHour()
    {
        (await _client.GetAsync("/api/v1/Country/Colombia")).EnsureSuccessStatusCode();

        var metrics = await _client.GetJsonAsync<JsonElement>("/api/v1/metrics");

        // A persisted hour beats anything served in the current hour.
        Assert.Equal(PersistedMax, metrics.GetProperty("largestResponseBytes").GetInt64());
        Assert.Equal(PersistedPath, metrics.GetProperty("largestResponsePath").GetString());

        // Monthly usage is grouped by month, newest first, and summed.
        var monthly = metrics.GetProperty("monthlyUsage").EnumerateArray().ToList();
        Assert.Equal(2, monthly.Count);

        Assert.Equal(2026, monthly[0].GetProperty("year").GetInt32());
        Assert.Equal(10, monthly[0].GetProperty("month").GetInt32());
        Assert.Equal(7, monthly[0].GetProperty("requestCount").GetInt64());
        Assert.Equal(70, monthly[0].GetProperty("totalResponseBytes").GetInt64());

        Assert.Equal(9, monthly[1].GetProperty("month").GetInt32());
        Assert.Equal(150, monthly[1].GetProperty("requestCount").GetInt64());
        Assert.Equal(1_500, monthly[1].GetProperty("totalResponseBytes").GetInt64());

        // The current hour is still reported from memory.
        Assert.True(metrics.GetProperty("currentHourRequestCount").GetInt64() > 0);
    }
}
