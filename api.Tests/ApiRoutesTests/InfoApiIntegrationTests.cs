using System.Net;

namespace api.Tests.ApiRoutesTests;

public class InfoApiIntegrationTests(CustomWebApplicationFactory factory) : IClassFixture<CustomWebApplicationFactory>
{
    private readonly HttpClient _client = factory.CreateClient();

#if DEBUG
    [Fact]
    public async Task DbCreation_Endpoint_ReturnsOk()
    {
        var response = await _client.GetAsync("/dbcreation");
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
    [Fact(Skip = "/dbcreation only available in DEBUG builds")] 
    public void DbCreation_Skipped_InRelease()
    {
        // Intentionally skipped in Release configuration
    }
#endif
}
