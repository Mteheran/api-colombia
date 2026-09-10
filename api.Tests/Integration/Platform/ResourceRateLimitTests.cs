using System.Net;
using api.Tests.Infrastructure;

namespace api.Tests.Integration.Platform;

/// <summary>
/// Holiday, City and Department all opt into the SAME named policy
/// (<c>Util.PublicRateLimitPolicy</c>, partitioned by IP only), so one client shares a single
/// budget across the three resources rather than getting one each.
/// </summary>
public class ResourceRateLimitTests : IClassFixture<RateLimitedFactory>
{
    // The three groups that opt into the shared per-IP policy. Every URL here
    // returns 200 against the seeded in-memory database.
    private static readonly string[] SharedEndpoints =
    [
        "/api/v1/Holiday/year/2026",
        "/api/v1/City",
        "/api/v1/Department",
    ];

    private readonly HttpClient _client;

    public ResourceRateLimitTests(RateLimitedFactory factory)
    {
        factory.EnsureSeeded();
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task Holiday_City_And_Department_ShareASinglePerIpBudget()
    {
        // Interleave the three resources until the shared budget is exhausted.
        // Because they share one bucket, the total allowed across all three is the budget.
        for (var i = 0; i < RateLimitedFactory.Permits; i++)
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
