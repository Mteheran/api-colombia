using System.Net.Http.Json;
using api.Models;
using api.Utils;

public class TraditionalFairAndFestivalApiIntegrationTests : IClassFixture<CustomWebApplicationFactory>
{
    private readonly HttpClient _client;

    public TraditionalFairAndFestivalApiIntegrationTests(CustomWebApplicationFactory factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task GetTraditionalFairAndFestivals_ReturnsOkWithExpectedData()
    {
        var response = await _client.GetAsync("/api/v1/TraditionalFairAndFestival");

        response.EnsureSuccessStatusCode();

       var result = await response.Content.ReadFromJsonAsync<List<TraditionalFairAndFestival>>(); 

        Assert.NotNull(result);
        Assert.NotEmpty(result);
    }

    [Fact]
    public async Task GetTraditionalFairAndFestivalById_ReturnsOkWithData()
    {
        int id = 1; 
        var response = await _client.GetAsync($"/api/v1/TraditionalFairAndFestival/{id}");

        response.EnsureSuccessStatusCode();

       var result = await response.Content.ReadFromJsonAsync<TraditionalFairAndFestival>(); 

        Assert.NotNull(result);
        Assert.Equal(id, result.Id);
        Assert.Equal("Sample Festival", result.Name);
    }

    [Fact]
    public async Task GetTraditionalFairAndFestivalByCity_ReturnsOkWithFilteredData()
    {
        int cityId = 1;  
        var response = await _client.GetAsync($"/api/v1/TraditionalFairAndFestival/{cityId}/city");

        response.EnsureSuccessStatusCode();

       var result = await response.Content.ReadFromJsonAsync<List<TraditionalFairAndFestival>>(); 

        Assert.NotNull(result);
        Assert.NotEmpty(result);
        Assert.All(result, item => Assert.Equal(cityId, item.CityId));
        Assert.Equal(2, result.Count);
    }

    [Fact]
    public async Task GetTraditionalFairAndFestivalByName_ReturnsOkWithExpectedData()
    {
        string name = "Sample Festival";  
        var response = await _client.GetAsync($"/api/v1/TraditionalFairAndFestival/name/{name}");

        response.EnsureSuccessStatusCode();

       var result = await response.Content.ReadFromJsonAsync<List<TraditionalFairAndFestival>>(); 

        Assert.NotNull(result);
        Assert.NotEmpty(result);
        Assert.Single(result);
    }

    [Fact]
    public async Task SearchTraditionalFairAndFestivals_ReturnsOkWithFilteredResults()
    {
        string keyword = "festi";  
        var response = await _client.GetAsync($"/api/v1/TraditionalFairAndFestival/search/{keyword}");

        response.EnsureSuccessStatusCode();

       var result = await response.Content.ReadFromJsonAsync<List<TraditionalFairAndFestival>>(); 

        Assert.NotNull(result);
        Assert.NotEmpty(result);
        Assert.Equal(2, result.Count);
    }

    [Fact]
    public async Task GetPagedTraditionalFairAndFestivals_ReturnsOkWithPagedData()
    {
        int page = 1;
        int pageSize = 2;

        var response = await _client.GetAsync($"/api/v1/TraditionalFairAndFestival/pagedList?page={page}&pageSize={pageSize}");

        response.EnsureSuccessStatusCode();

        var result = await response.Content.ReadFromJsonAsync<PaginationResponseModel<TraditionalFairAndFestival>>(); 

        Assert.NotNull(result);
        Assert.Equal(pageSize, result.PageSize);
        Assert.Equal(page, result.Page);
        Assert.Equal(pageSize, result.Data.Count);
        Assert.Equal(3, result.TotalRecords);
    }
}