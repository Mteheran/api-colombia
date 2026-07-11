using System.Net;

namespace api.Tests.ApiRoutesTests;

// Separate test class so it gets its own CustomWebApplicationFactory instance
// (and therefore its own in-memory rate-limiter state), keeping the burst of
// requests below from tripping the limit for the other Holiday tests.
public class HolidayRateLimitTests : IClassFixture<CustomWebApplicationFactory>
{
    private readonly HttpClient _client;

    public HolidayRateLimitTests(CustomWebApplicationFactory factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task GetHolidays_ByYear_ExceedingPerIpLimit_Returns429WithRetryAfter()
    {
        const string url = "/api/v1/Holiday/year/2026";
        const int permitLimit = 60; // matches SlidingWindowRateLimiterOptions.PermitLimit

        // The first 60 requests (same IP, same window) must be allowed.
        for (var i = 0; i < permitLimit; i++)
        {
            var allowed = await _client.GetAsync(url);
            Assert.Equal(HttpStatusCode.OK, allowed.StatusCode);
        }

        // The 61st request in the window is rejected.
        var rejected = await _client.GetAsync(url);

        Assert.Equal(HttpStatusCode.TooManyRequests, rejected.StatusCode);
        Assert.True(rejected.Headers.RetryAfter is not null, "Expected a Retry-After header on the 429 response.");
    }
}
