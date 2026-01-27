using System.Net.Http.Json;
using api.Models;
using api.Utils;

namespace api.Tests.ApiRoutesTests;

public class HeritageCityApiIntegrationTests : IClassFixture<CustomWebApplicationFactory>
{
    private readonly HttpClient _client;

    public HeritageCityApiIntegrationTests(CustomWebApplicationFactory factory)
    {
        factory.ResetDatabase();
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task GetHeritageCities_ReturnsOkWithExpectedData()
    {
        var response = await _client.GetAsync("/api/v1/HeritageCity");

        response.EnsureSuccessStatusCode();

        var result = await response.Content.ReadFromJsonAsync<List<HeritageCity>>();

        Assert.NotNull(result);
        Assert.NotEmpty(result);
        Assert.Equal(3, result.Count);
    }

    [Fact]
    public async Task GetHeritageCityById_ReturnsOkWithData()
    {
        int id = 1;
        var response = await _client.GetAsync($"/api/v1/HeritageCity/{id}");

        response.EnsureSuccessStatusCode();

        var result = await response.Content.ReadFromJsonAsync<HeritageCity>();

        Assert.NotNull(result);
        Assert.Equal(id, result.Id);
        Assert.Equal("Cartagena", result.Name);
    }

    [Fact]
    public async Task GetHeritageCityById_ReturnsBadRequest()
    {
        int id = 0;
        var response = await _client.GetAsync($"/api/v1/HeritageCity/{id}");

        Assert.False(response.IsSuccessStatusCode);
        Assert.Equal(System.Net.HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task GetHeritageCityByName_ReturnsOkWithExpectedData()
    {
        string name = "Cartagena";
        var response = await _client.GetAsync($"/api/v1/HeritageCity/name/{name}");

        response.EnsureSuccessStatusCode();

        var result = await response.Content.ReadFromJsonAsync<List<HeritageCity>>();

        Assert.NotNull(result);
        Assert.NotEmpty(result);
        Assert.Single(result);
        Assert.Equal(name, result[0].Name);
    }

    [Fact]
    public async Task SearchHeritageCities_ReturnsOkWithFilteredResults()
    {
        string keyword = "mompox";
        var response = await _client.GetAsync($"/api/v1/HeritageCity/search/{keyword}");

        response.EnsureSuccessStatusCode();

        var result = await response.Content.ReadFromJsonAsync<List<HeritageCity>>();

        Assert.NotNull(result);
        Assert.NotEmpty(result);
        Assert.Single(result);
        Assert.Contains(keyword, result[0].Name, StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public async Task GetPagedHeritageCities_ReturnsOkWithPagedData()
    {
        int page = 1;
        int pageSize = 2;

        var response = await _client.GetAsync($"/api/v1/HeritageCity/pagedList?page={page}&pageSize={pageSize}");

        response.EnsureSuccessStatusCode();

        var result = await response.Content.ReadFromJsonAsync<PaginationResponseModel<HeritageCity>>();

        Assert.NotNull(result);
        Assert.Equal(pageSize, result.PageSize);
        Assert.Equal(page, result.Page);
        Assert.Equal(pageSize, result.Data.Count);
        Assert.Equal(3, result.TotalRecords);
    }
}
