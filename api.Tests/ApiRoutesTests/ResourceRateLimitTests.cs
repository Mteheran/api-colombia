using System.Net;

namespace api.Tests.ApiRoutesTests;

// Separate test class → its own CustomWebApplicationFactory instance, so this
// burst doesn't interfere with the other resource tests. Holiday, City and
// Department all reuse the SAME named policy (Util.PublicRateLimitPolicy,
// partitioned by IP only), so a single client shares ONE 60/min budget across
// the three resources.
public class ResourceRateLimitTests : IClassFixture<CustomWebApplicationFactory>
{
    private const int PermitLimit = 60; // matches SlidingWindowRateLimiterOptions.PermitLimit

    // The three groups that opt into the shared per-IP policy. Every URL here
    // returns 200 with the seeded in-memory database.
    private static readonly string[] SharedEndpoints =
    {
        "/api/v1/Holiday/year/2026",
        "/api/v1/City",
        "/api/v1/Department",
    };

    private readonly HttpClient _client;

    public ResourceRateLimitTests(CustomWebApplicationFactory factory)
    {
        factory.ResetDatabase();
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task Holiday_City_And_Department_ShareASinglePerIpBudget()
    {
        // Interleave the three resources until the shared budget is exhausted.
        // Because they share one bucket, the total allowed across all three is 60.
        for (var i = 0; i < PermitLimit; i++)
        {
            var url = SharedEndpoints[i % SharedEndpoints.Length];
            var allowed = await _client.GetAsync(url);
            Assert.Equal(HttpStatusCode.OK, allowed.StatusCode);
        }

        // The next request on ANY of the shared groups is rejected — proving the
        // budget is shared, not per-resource.
        foreach (var url in SharedEndpoints)
        {
            var rejected = await _client.GetAsync(url);
            Assert.Equal(HttpStatusCode.TooManyRequests, rejected.StatusCode);
            Assert.True(rejected.Headers.RetryAfter is not null,
                $"Expected a Retry-After header on the 429 response for {url}.");
        }
    }
}
