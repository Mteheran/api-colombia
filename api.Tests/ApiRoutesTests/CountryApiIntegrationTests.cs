using api.Models;
using System.Net.Http.Json;

public class CountryApiIntegrationTests : IClassFixture<CustomWebApplicationFactory>
{
    private readonly HttpClient _client;

    public CountryApiIntegrationTests(CustomWebApplicationFactory factory)
    {
        _client = factory.CreateClient();  
    }

    [Fact]
    public async Task GetCountry_ReturnsAllCountryData()
    {
        var response = await _client.GetAsync("/api/v1/Country/Colombia");

        response.EnsureSuccessStatusCode();

        var result =  await response.Content.ReadFromJsonAsync<Country>();
 
        Assert.NotNull(result);
        Assert.Equal("Colombia", result.Name);
        Assert.Equal("Americas", result.Region);
        Assert.Equal("COP", result.CurrencyCode);
        Assert.Equal("$", result.CurrencySymbol);
        Assert.Equal("Peso", result.Currency);
    }
}
