using System.Net;
using System.Text.Json;
using api.Tests.Infrastructure;

namespace api.Tests.Integration.Platform;

/// <summary>
/// Kept on an <see cref="IsolatedFactory"/>: these assertions are about what the collector
/// recorded on THIS host, and every other test's traffic would be noise in them.
/// </summary>
public class MetricsApiIntegrationTests : IClassFixture<IsolatedFactory>
{
    private readonly HttpClient _client;

    public MetricsApiIntegrationTests(IsolatedFactory factory)
    {
        factory.EnsureSeeded();
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task GetMetrics_ReturnsOk_WithExpectedSchemaAndRecordedRequest()
    {
        // Make a distinctive request first so the collector has something to report.
        var dataResponse = await _client.GetAsync("/api/v1/Country/Colombia");
        dataResponse.EnsureSuccessStatusCode();

        var response = await _client.GetAsync("/api/v1/metrics");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        using var json = JsonDocument.Parse(await response.Content.ReadAsStringAsync());
        var root = json.RootElement;

        // Expected top-level shape (camelCase from System.Text.Json).
        Assert.True(root.TryGetProperty("generatedAtUtc", out _));
        Assert.True(root.TryGetProperty("currentHourRequestCount", out var currentCount));
        Assert.True(root.TryGetProperty("largestResponseBytes", out _));
        Assert.True(root.TryGetProperty("recentRequests", out var recent));
        Assert.True(root.TryGetProperty("monthlyUsage", out var monthly));

        Assert.Equal(JsonValueKind.Array, recent.ValueKind);
        Assert.Equal(JsonValueKind.Array, monthly.ValueKind);

        // At least the country request should have been recorded.
        Assert.True(currentCount.GetInt64() > 0);
        Assert.Contains(recent.EnumerateArray(),
            r => r.GetProperty("path").GetString()!.Contains("Country", StringComparison.OrdinalIgnoreCase));

        // Each recent entry exposes the analytics fields.
        var first = recent.EnumerateArray().First();
        Assert.True(first.TryGetProperty("method", out _));
        Assert.True(first.TryGetProperty("statusCode", out _));
        Assert.True(first.TryGetProperty("responseBytes", out _));
    }
}
