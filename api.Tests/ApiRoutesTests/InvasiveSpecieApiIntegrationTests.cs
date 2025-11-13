using System.Net.Http.Json;
using api.Models;
using api.Utils;

namespace api.Tests.ApiRoutesTests;

public class InvasiveSpecieApiIntegrationTests(CustomWebApplicationFactory factory)
    : IClassFixture<CustomWebApplicationFactory>
{
    private readonly HttpClient _client = factory.CreateClient();

    [Fact]
    public async Task GetInvasiveSpecies_ReturnsOkWithExpectedData()
    {
        var response = await _client.GetAsync("/api/v1/InvasiveSpecie");

        response.EnsureSuccessStatusCode();

        var result = await response.Content.ReadFromJsonAsync<List<InvasiveSpecie>>();

        Assert.NotNull(result);    
        Assert.Equal(3, result.Count);
    }

    [Fact]
    public async Task GetInvasiveSpecieById_ReturnsOkWithSpecieData()
    {
        int id = 1;  
        var response = await _client.GetAsync($"/api/v1/InvasiveSpecie/{id}");

        response.EnsureSuccessStatusCode();

        var result = await response.Content.ReadFromJsonAsync<InvasiveSpecie>();

        Assert.NotNull(result);
        Assert.Equal(id, result.Id);
    }

    [Fact]
    public async Task GetInvasiveSpecieById_ReturnsBadRequest()
    {
        int id = 0;  
        var response = await _client.GetAsync($"/api/v1/InvasiveSpecie/{id}");

        Assert.False(response.IsSuccessStatusCode);
        Assert.Equal(System.Net.HttpStatusCode.BadRequest, response.StatusCode);
    }


    [Fact]
    public async Task GetInvasiveSpecieByName_ReturnsOkWithSpecieData()
    {
        string name = "Sample Specie";  
        var response = await _client.GetAsync($"/api/v1/InvasiveSpecie/name/{name}");

        response.EnsureSuccessStatusCode();

        var result = await response.Content.ReadFromJsonAsync<List<InvasiveSpecie>>();

        Assert.NotNull(result);
        Assert.Equal(name, result[0].Name);
    }

    [Fact]
    public async Task SearchInvasiveSpecies_ReturnsOkWithFilteredSpecies()
    {
        string keyword = "Specie";  
        var response = await _client.GetAsync($"/api/v1/InvasiveSpecie/search/{keyword}");

        response.EnsureSuccessStatusCode();

        var result = await response.Content.ReadFromJsonAsync<List<InvasiveSpecie>>();

        Assert.NotNull(result);
        Assert.Equal(3, result.Count);
        Assert.All(result, specie => Assert.Contains(keyword, specie.Name));
    }

    [Fact]
    public async Task GetPagedInvasiveSpecies_ReturnsOkWithPagedResults()
    {
        int page = 1;
        int pageSize = 1;
        var response = await _client.GetAsync($"/api/v1/InvasiveSpecie/pagedList?page={page}&pageSize={pageSize}");

        response.EnsureSuccessStatusCode();

        var result = await response.Content.ReadFromJsonAsync<PaginationResponseModel<InvasiveSpecie>>();

        Assert.NotNull(result);
        Assert.Equal(page, result.Page);
        Assert.Equal(pageSize, result.PageSize);
        Assert.Equal(3, result.TotalRecords);
    }

    [Fact]
    public async Task GetInvasiveSpeciesWithSorting_ReturnsOkWithSortedData()
    {
        string sortBy = "Name";  
        string sortDirection = "desc";  

        var response = await _client.GetAsync($"/api/v1/InvasiveSpecie?sortBy={sortBy}&sortDirection={sortDirection}");

        response.EnsureSuccessStatusCode();

        var result = await response.Content.ReadFromJsonAsync<List<InvasiveSpecie>>();

        Assert.NotNull(result);
        Assert.Equal(3, result.Count);
        Assert.Equal("Third Specie", result[0].Name);  
    }
}