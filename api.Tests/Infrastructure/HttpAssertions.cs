using System.Net;
using System.Net.Http.Json;

namespace api.Tests.Infrastructure;

/// <summary>
/// The three things nearly every test does: fetch and deserialize a 200, or assert a 400/404.
/// Keeps the intent on one line so the interesting part of a test is the URL and the assertion.
/// </summary>
public static class HttpAssertions
{
    /// <summary>GETs <paramref name="url"/>, asserts success, and deserializes the body.</summary>
    public static async Task<T> GetJsonAsync<T>(this HttpClient client, string url)
    {
        var response = await client.GetAsync(url);

        response.EnsureSuccessStatusCode();

        var result = await response.Content.ReadFromJsonAsync<T>();

        Assert.NotNull(result);
        return result;
    }

    public static async Task AssertStatusAsync(this HttpClient client, string url, HttpStatusCode expected)
    {
        var response = await client.GetAsync(url);

        Assert.Equal(expected, response.StatusCode);
    }

    public static Task AssertBadRequestAsync(this HttpClient client, string url) =>
        client.AssertStatusAsync(url, HttpStatusCode.BadRequest);

    public static Task AssertNotFoundAsync(this HttpClient client, string url) =>
        client.AssertStatusAsync(url, HttpStatusCode.NotFound);
}
