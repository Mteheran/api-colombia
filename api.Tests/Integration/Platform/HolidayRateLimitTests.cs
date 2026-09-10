using System.Net;
using api.Tests.Infrastructure;

namespace api.Tests.Integration.Platform;

/// <summary>
/// Uses its own <see cref="RateLimitedFactory"/> — and therefore its own limiter state — so the
/// burst below cannot trip the limit for any other test. The factory configures a deliberately
/// tiny budget, which proves the same behaviour in a handful of requests instead of 61.
/// </summary>
public class HolidayRateLimitTests(RateLimitedFactory factory) : IClassFixture<RateLimitedFactory>
{
    private readonly HttpClient _client = factory.CreateClient();

    [Fact]
    public async Task GetHolidays_ByYear_ExceedingPerIpLimit_Returns429WithRetryAfter()
    {
        const string url = "/api/v1/Holiday/year/2026";

        // Every request inside the budget (same IP, same window) must be allowed.
        for (var i = 0; i < RateLimitedFactory.Permits; i++)
        {
            var allowed = await _client.GetAsync(url);
            Assert.Equal(HttpStatusCode.OK, allowed.StatusCode);
        }

        // The next request in the window is rejected.
        var rejected = await _client.GetAsync(url);

        Assert.Equal(HttpStatusCode.TooManyRequests, rejected.StatusCode);
        Assert.True(rejected.Headers.RetryAfter is not null, "Expected a Retry-After header on the 429 response.");
    }
}
