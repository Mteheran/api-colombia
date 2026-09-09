using System.Net;
using api.Tests.Infrastructure;
using Microsoft.AspNetCore.Mvc.Testing;

namespace api.Tests.Integration.Platform;

/// <summary>
/// Covers the DEBUG-only `/db-creation` endpoint (<c>api/Routes/InfoRoutes.cs</c>).
///
/// This used to request `/dbcreation` — a route that does not exist — and pass anyway:
/// `UseStatusCodePages` redirects a 404 outside `/api` to `/`, the default client follows the
/// redirect, and the static landing page answers 200. The misspelling now has its own test that
/// asserts the redirect explicitly, so the real endpoint can be checked on its real path.
/// </summary>
public class InfoApiIntegrationTests(IsolatedFactory factory) : IClassFixture<IsolatedFactory>
{
    private readonly HttpClient _client = factory.CreateClient();

    private readonly HttpClient _noRedirectClient = factory.CreateClient(
        new WebApplicationFactoryClientOptions { AllowAutoRedirect = false });

#if DEBUG
    [Fact]
    public async Task DbCreation_Endpoint_ReturnsOk()
    {
        var response = await _client.GetAsync("/db-creation");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task DbCreation_CanBeCalledTwice_ReturnsOkBothTimes()
    {
        var first = await _client.GetAsync("/db-creation");
        var second = await _client.GetAsync("/db-creation");

        Assert.Equal(HttpStatusCode.OK, first.StatusCode);
        Assert.Equal(HttpStatusCode.OK, second.StatusCode);
    }
#else
    [Fact(Skip = "/db-creation is only registered in DEBUG builds")]
    public void DbCreation_Skipped_InRelease()
    {
        // Intentionally skipped in Release configuration
    }
#endif

    [Fact]
    public async Task MisspelledDbCreation_IsNotAnEndpoint_AndRedirectsToTheLandingPage()
    {
        var response = await _noRedirectClient.GetAsync("/dbcreation");

        Assert.Equal(HttpStatusCode.Redirect, response.StatusCode);
        Assert.Equal("/", response.Headers.Location?.OriginalString);
    }
}
