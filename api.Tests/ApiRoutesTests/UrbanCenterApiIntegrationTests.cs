using System.Net.Http.Json;
using api.Models;
using api.Utils;

namespace api.Tests.ApiRoutesTests;

public class UrbanCenterApiIntegrationTests : IClassFixture<CustomWebApplicationFactory>
{
    private readonly HttpClient _client;

    public UrbanCenterApiIntegrationTests(CustomWebApplicationFactory factory)
    {
        factory.ResetDatabase();
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task GetUrbanCenters_ReturnsOkWithExpectedData()
    {
        var response = await _client.GetAsync("/api/v1/UrbanCenter");

        response.EnsureSuccessStatusCode();

        var result = await response.Content.ReadFromJsonAsync<List<UrbanCenter>>();

        Assert.NotNull(result);
        Assert.NotEmpty(result);
        Assert.Equal(3, result.Count);
    }

    [Fact]
    public async Task GetUrbanCenterById_ReturnsOkWithData()
    {
        int id = 1;
        var response = await _client.GetAsync($"/api/v1/UrbanCenter/{id}");

        response.EnsureSuccessStatusCode();

        var result = await response.Content.ReadFromJsonAsync<UrbanCenter>();

        Assert.NotNull(result);
        Assert.Equal(id, result.Id);
        Assert.Equal("001", result.Code);
    }

    [Fact]
    public async Task GetUrbanCenterById_ReturnsBadRequest()
    {
        int id = 0;
        var response = await _client.GetAsync($"/api/v1/UrbanCenter/{id}");

        Assert.False(response.IsSuccessStatusCode);
        Assert.Equal(System.Net.HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task GetUrbanCenterByCode_ReturnsOkWithExpectedData()
    {
        string code = "001";
        var response = await _client.GetAsync($"/api/v1/UrbanCenter/code/{code}");

        response.EnsureSuccessStatusCode();

        var result = await response.Content.ReadFromJsonAsync<List<UrbanCenter>>();

        Assert.NotNull(result);
        Assert.NotEmpty(result);
        Assert.Single(result);
        Assert.Equal(code, result[0].Code);
    }

    [Fact]
    public async Task GetUrbanCenterByCity_ReturnsOkWithExpectedData()
    {
        int cityId = 1;
        var response = await _client.GetAsync($"/api/v1/UrbanCenter/city/{cityId}");

        response.EnsureSuccessStatusCode();

        var result = await response.Content.ReadFromJsonAsync<List<UrbanCenter>>();

        Assert.NotNull(result);
        Assert.NotEmpty(result);
        Assert.Equal(2, result.Count);
    }

    [Fact]
    public async Task SearchUrbanCenters_ReturnsOkWithFilteredResults()
    {
        string keyword = "Poblado";
        var response = await _client.GetAsync($"/api/v1/UrbanCenter/search/{keyword}");

        response.EnsureSuccessStatusCode();

        var result = await response.Content.ReadFromJsonAsync<List<UrbanCenter>>();

        Assert.NotNull(result);
        Assert.NotEmpty(result);
        Assert.Single(result);
        Assert.Contains(keyword, result[0].Name, StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public async Task GetPagedUrbanCenters_ReturnsOkWithPagedData()
    {
        int page = 1;
        int pageSize = 2;

        var response = await _client.GetAsync($"/api/v1/UrbanCenter/pagedList?page={page}&pageSize={pageSize}");

        response.EnsureSuccessStatusCode();

        var result = await response.Content.ReadFromJsonAsync<PaginationResponseModel<UrbanCenter>>();

        Assert.NotNull(result);
        Assert.Equal(pageSize, result.PageSize);
        Assert.Equal(page, result.Page);
        Assert.Equal(pageSize, result.Data.Count);
        Assert.Equal(3, result.TotalRecords);
    }
}
