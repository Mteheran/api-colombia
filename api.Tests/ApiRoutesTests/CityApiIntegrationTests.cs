using System.Net.Http;
using System.Threading.Tasks;
using Xunit;
using Microsoft.AspNetCore.Mvc.Testing;

public class CityApiIntegrationTests : IClassFixture<CustomWebApplicationFactory> , IDisposable
{
    private readonly HttpClient _client;

    public CityApiIntegrationTests(CustomWebApplicationFactory factory)
    {
       _client = new CustomWebApplicationFactory().CreateClient(); 
    }

    public void Dispose()
    {
        _client.Dispose();
    }

    [Fact]
    public async Task GetCities_ReturnsOkWithExpectedData()
    {   
        var response = await _client.GetAsync("/api/v1/City");

        response.EnsureSuccessStatusCode();

        var result = await response.Content.ReadAsStringAsync();

        Assert.NotNull(result);
        Assert.False(string.IsNullOrEmpty(result));  
    }

    [Fact]
    public async Task GetCityById_ReturnsOkWithCityData()
    {
        int cityId = 1;  
        var response = await _client.GetAsync($"/api/v1/City/{cityId}");

        response.EnsureSuccessStatusCode();

        var result = await response.Content.ReadAsStringAsync();

        Assert.NotNull(result);
        Assert.Contains("Id", result);
    }

    [Fact]
    public async Task GetCityByName_ReturnsOkWithCityData()
    {
        string cityName = "Medellín";  
        var response = await _client.GetAsync($"/api/v1/City/name/{cityName}");

        response.EnsureSuccessStatusCode();

        var result = await response.Content.ReadAsStringAsync();

        Assert.NotNull(result);
        Assert.Contains("name", result);
    }

    [Fact]
    public async Task SearchCities_ReturnsOkWithFilteredData()
    {
        string keyword = "Medellín";  
        var response = await _client.GetAsync($"/api/v1/City/search/{keyword}");

        response.EnsureSuccessStatusCode();

        var result = await response.Content.ReadAsStringAsync();

        Assert.NotNull(result);
        Assert.Contains("Medellín", result);
    }

    [Fact]
    public async Task GetCitiesPagedList_ReturnsOkWithPagedData()
    {
        var paginationParams = "?page=1&pageSize=10";
        var response = await _client.GetAsync($"/api/v1/City/pagedList{paginationParams}");

        response.EnsureSuccessStatusCode();

        var result = await response.Content.ReadAsStringAsync();

        Assert.NotNull(result);
        Assert.Contains("page", result);
        Assert.Contains("pageSize", result);
    }

    [Fact]
    public async Task GetCitiesWithSorting_ReturnsOkWithSortedData()
    {
        string sortBy = "Name"; 
        string sortDirection = "asc";  

        var response = await _client.GetAsync($"/api/v1/City?sortBy={sortBy}&sortDirection={sortDirection}");

        response.EnsureSuccessStatusCode();

        var result = await response.Content.ReadAsStringAsync();

        Assert.NotNull(result);
        Assert.Contains("name", result);  
    }
}
