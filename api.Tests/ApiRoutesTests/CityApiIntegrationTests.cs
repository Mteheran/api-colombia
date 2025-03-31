using System.Net.Http.Json;
using api.Models;
using api.Utils;

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

        var result = await response.Content.ReadFromJsonAsync<List<City>>(); 

        Assert.NotNull(result);
        Assert.Equal(2, result.Count);
    }

    [Fact]
    public async Task GetCityById_ReturnsOkWithCityData()
    {
        int cityId = 1;  
        var response = await _client.GetAsync($"/api/v1/City/{cityId}");

        response.EnsureSuccessStatusCode();

        var result = await response.Content.ReadFromJsonAsync<City>(); 

        Assert.NotNull(result);
        Assert.Equal(cityId, result.Id);
    }

    [Fact]
    public async Task GetCityByName_ReturnsOkWithCityData()
    {
        string cityName = "Medellín";  
        var response = await _client.GetAsync($"/api/v1/City/name/{cityName}");

        response.EnsureSuccessStatusCode();

        var result = await response.Content.ReadFromJsonAsync<List<City>>();

        Assert.NotNull(result);
        Assert.Equal(cityName, result[0].Name);
    }

    [Fact]
    public async Task SearchCities_ReturnsOkWithFilteredData()
    {
        string keyword = "Medellín";  
        var response = await _client.GetAsync($"/api/v1/City/search/{keyword}");

        response.EnsureSuccessStatusCode();

        var result = await response.Content.ReadFromJsonAsync<List<City>>();

        Assert.NotNull(result);
        Assert.Single(result);
        Assert.All(result, city => Assert.Contains(keyword, city.Name, StringComparison.OrdinalIgnoreCase));
    }

    [Fact]
    public async Task GetCitiesPagedList_ReturnsOkWithPagedData()
    {
        var paginationParams = "?page=1&pageSize=10";
        var response = await _client.GetAsync($"/api/v1/City/pagedList{paginationParams}");

        response.EnsureSuccessStatusCode();
        var result = await response.Content.ReadFromJsonAsync<PaginationResponseModel<City>>();

        Assert.NotNull(result);
        Assert.Equal(10, result.PageSize);
        Assert.Equal(1, result.Page);
        Assert.Equal(2, result.TotalRecords);
        Assert.Equal(2, result.Data.Count);
    }

    [Fact]
    public async Task GetCitiesWithSorting_ReturnsOkWithSortedData()
    {
        string sortBy = "Name"; 
        string sortDirection = "asc";  

        var response = await _client.GetAsync($"/api/v1/City?sortBy={sortBy}&sortDirection={sortDirection}");

        response.EnsureSuccessStatusCode();

        var result = await response.Content.ReadFromJsonAsync<List<City>>();

        Assert.NotNull(result); 
        Assert.Equal(2, result.Count);
        Assert.Equal("Cali", result[0].Name);
        Assert.Equal("Medellín", result[1].Name);
    }
}
