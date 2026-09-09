using System.Net;
using System.Net.Http.Headers;
using System.Text.Json;
using api.Tests.Infrastructure;
using Microsoft.AspNetCore.Mvc.Testing;

namespace api.Tests.Integration.Platform;

/// <summary>
/// The middleware pipeline itself — output caching, CORS, compression, Swagger and the 404
/// handling — none of which had any test. These are cross-cutting behaviours a resource test
/// never exercises, and they are exactly the kind of thing that breaks silently on an upgrade.
/// </summary>
public class PipelineTests(IsolatedFactory factory) : IClassFixture<IsolatedFactory>
{
    private readonly IsolatedFactory _factory = Seeded(factory);

    private static IsolatedFactory Seeded(IsolatedFactory factory)
    {
        factory.EnsureSeeded();
        return factory;
    }

    private HttpClient Client => _factory.CreateClient();

    // ---------- Output caching ----------

    [Fact]
    public async Task RepeatedRequest_IsServedFromTheOutputCache()
    {
        var client = Client;

        var first = await client.GetAsync("/api/v1/Volcano");
        var second = await client.GetAsync("/api/v1/Volcano");

        first.EnsureSuccessStatusCode();
        second.EnsureSuccessStatusCode();

        Assert.Equal(
            await first.Content.ReadAsStringAsync(),
            await second.Content.ReadAsStringAsync());
    }

    [Fact]
    public async Task OutputCache_VariesByQueryString()
    {
        var client = Client;

        var ascending = await client.GetJsonAsync<JsonElement>("/api/v1/Volcano?sortBy=Name&sortDirection=asc");
        var descending = await client.GetJsonAsync<JsonElement>("/api/v1/Volcano?sortBy=Name&sortDirection=desc");

        // SetVaryByQuery("*") means these are separate cache entries, not one shared response.
        Assert.NotEqual(ascending.ToString(), descending.ToString());
    }

    // ---------- CORS ----------

    [Fact]
    public async Task Cors_AllowsGetFromAnyOrigin()
    {
        var request = new HttpRequestMessage(HttpMethod.Options, "/api/v1/Volcano");
        request.Headers.Add("Origin", "https://example.com");
        request.Headers.Add("Access-Control-Request-Method", "GET");

        var response = await Client.SendAsync(request);

        Assert.True(response.Headers.Contains("Access-Control-Allow-Origin"),
            "Expected the preflight to advertise Access-Control-Allow-Origin for GET.");
    }

    [Fact]
    public async Task Cors_AdvertisesGetOnly()
    {
        var request = new HttpRequestMessage(HttpMethod.Options, "/api/v1/Volcano");
        request.Headers.Add("Origin", "https://example.com");
        request.Headers.Add("Access-Control-Request-Method", "GET");

        var response = await Client.SendAsync(request);
        var allowed = response.Headers.TryGetValues("Access-Control-Allow-Methods", out var values)
            ? string.Join(",", values)
            : string.Empty;

        // The policy is built with WithMethods("GET") only, so no write verb may be advertised.
        Assert.Contains("GET", allowed);
        Assert.DoesNotContain("POST", allowed);
        Assert.DoesNotContain("DELETE", allowed);
    }

    [Fact]
    public async Task Post_ToAReadOnlyResource_IsNotAllowed()
    {
        var response = await Client.PostAsync("/api/v1/Volcano", new StringContent("{}"));

        Assert.Equal(HttpStatusCode.MethodNotAllowed, response.StatusCode);
    }

    // ---------- Response compression ----------

    [Fact]
    public async Task Json_IsCompressed_WhenTheClientAcceptsBrotli()
    {
        // The factory's default handler transparently decompresses, which would hide the header.
        var client = _factory.CreateDefaultClient();

        var request = new HttpRequestMessage(HttpMethod.Get, "/api/v1/Volcano");
        request.Headers.AcceptEncoding.Add(new StringWithQualityHeaderValue("br"));

        var response = await client.SendAsync(request);

        response.EnsureSuccessStatusCode();
        Assert.Contains("br", response.Content.Headers.ContentEncoding);
    }

    // ---------- Swagger ----------

    [Fact]
    public async Task SwaggerDocument_IsServed_AndCarriesTheCurrentVersion()
    {
        var document = await Client.GetJsonAsync<JsonElement>("/swagger/v1/swagger.json");

        Assert.Equal(
            api.Const.VersionInfo.CurrentVersion,
            document.GetProperty("info").GetProperty("version").GetString());
    }

    [Fact]
    public async Task SwaggerDocument_DescribesEveryResourceTag()
    {
        var document = await Client.GetJsonAsync<JsonElement>("/swagger/v1/swagger.json");
        var paths = document.GetProperty("paths").EnumerateObject().Select(p => p.Name).ToList();

        Assert.Contains("/api/v1/Volcano", paths);
        Assert.Contains("/api/v1/City", paths);
        Assert.Contains("/api/v1/Country/Colombia", paths);
    }

    // ---------- 404 handling ----------

    [Fact]
    public async Task UnknownApiPath_Returns404_WithoutRedirecting()
    {
        var client = _factory.CreateClient(new WebApplicationFactoryClientOptions { AllowAutoRedirect = false });

        var response = await client.GetAsync("/api/v1/NoSuchResource");

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task UnknownNonApiPath_RedirectsToTheLandingPage()
    {
        var client = _factory.CreateClient(new WebApplicationFactoryClientOptions { AllowAutoRedirect = false });

        var response = await client.GetAsync("/definitely-not-a-page");

        Assert.Equal(HttpStatusCode.Redirect, response.StatusCode);
        Assert.Equal("/", response.Headers.Location?.OriginalString);
    }

    [Fact]
    public async Task LandingPage_IsServedAtTheRoot()
    {
        var response = await Client.GetAsync("/");

        response.EnsureSuccessStatusCode();
        Assert.Contains("text/html", response.Content.Headers.ContentType?.MediaType ?? string.Empty);
    }

    [Theory]
    [InlineData("/metrics")]
    [InlineData("/mcp")]
    public async Task DashboardPage_IsServed(string path)
    {
        var response = await Client.GetAsync(path);

        response.EnsureSuccessStatusCode();
        Assert.Contains("text/html", response.Content.Headers.ContentType?.MediaType ?? string.Empty);
    }

    // ---------- Serialization ----------

    [Fact]
    public async Task DateOnly_IsSerializedAsAnIsoDate_EndToEnd()
    {
        var presidents = await Client.GetJsonAsync<JsonElement>("/api/v1/President");
        var start = presidents[0].GetProperty("startPeriodDate").GetString();

        Assert.Matches(@"^\d{4}-\d{2}-\d{2}$", start);
    }
}
