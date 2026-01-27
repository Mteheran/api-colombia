using System.Net.Http.Json;
using api.Models;
using api.Utils;

namespace api.Tests.ApiRoutesTests;

public class IntangibleHeritageApiIntegrationTests : IClassFixture<CustomWebApplicationFactory>
{
    private readonly HttpClient _client;

    public IntangibleHeritageApiIntegrationTests(CustomWebApplicationFactory factory)
    {
        factory.ResetDatabase();
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task GetIntangibleHeritages_ReturnsOkWithExpectedData()
    {
        var response = await _client.GetAsync("/api/v1/IntangibleHeritage");

        response.EnsureSuccessStatusCode();

        var result = await response.Content.ReadFromJsonAsync<List<IntangibleHeritage>>();

        Assert.NotNull(result);
        Assert.NotEmpty(result);
        Assert.Equal(3, result.Count);
    }

    [Fact]
    public async Task GetIntangibleHeritageById_ReturnsOkWithData()
    {
        int id = 1;
        var response = await _client.GetAsync($"/api/v1/IntangibleHeritage/{id}");

        response.EnsureSuccessStatusCode();

        var result = await response.Content.ReadFromJsonAsync<IntangibleHeritage>();

        Assert.NotNull(result);
        Assert.Equal(id, result.Id);
        Assert.Equal("Carnaval de Barranquilla", result.Name);
    }

    [Fact]
    public async Task GetIntangibleHeritageById_ReturnsBadRequest()
    {
        int id = 0;
        var response = await _client.GetAsync($"/api/v1/IntangibleHeritage/{id}");

        Assert.False(response.IsSuccessStatusCode);
        Assert.Equal(System.Net.HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task GetIntangibleHeritageByDepartment_ReturnsOkWithFilteredData()
    {
        int departmentId = 1;
        var response = await _client.GetAsync($"/api/v1/IntangibleHeritage/{departmentId}/department");

        response.EnsureSuccessStatusCode();

        var result = await response.Content.ReadFromJsonAsync<List<IntangibleHeritage>>();

        Assert.NotNull(result);
        Assert.NotEmpty(result);
        Assert.All(result, item => Assert.Equal(departmentId, item.DepartmentId));
        Assert.Equal(2, result.Count);
    }

    [Fact]
    public async Task GetIntangibleHeritageByName_ReturnsOkWithExpectedData()
    {
        string name = "Carnaval de Barranquilla";
        var response = await _client.GetAsync($"/api/v1/IntangibleHeritage/name/{name}");

        response.EnsureSuccessStatusCode();

        var result = await response.Content.ReadFromJsonAsync<List<IntangibleHeritage>>();

        Assert.NotNull(result);
        Assert.NotEmpty(result);
        Assert.Single(result);
        Assert.Equal(name, result[0].Name);
    }

    [Fact]
    public async Task SearchIntangibleHeritages_ReturnsOkWithFilteredResults()
    {
        string keyword = "carn";
        var response = await _client.GetAsync($"/api/v1/IntangibleHeritage/search/{keyword}");

        response.EnsureSuccessStatusCode();

        var result = await response.Content.ReadFromJsonAsync<List<IntangibleHeritage>>();

        Assert.NotNull(result);
        Assert.NotEmpty(result);
        Assert.Single(result);
        Assert.Contains(keyword, result[0].Name, StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public async Task GetPagedIntangibleHeritages_ReturnsOkWithPagedData()
    {
        int page = 1;
        int pageSize = 2;

        var response = await _client.GetAsync($"/api/v1/IntangibleHeritage/pagedList?page={page}&pageSize={pageSize}");

        response.EnsureSuccessStatusCode();

        var result = await response.Content.ReadFromJsonAsync<PaginationResponseModel<IntangibleHeritage>>();

        Assert.NotNull(result);
        Assert.Equal(pageSize, result.PageSize);
        Assert.Equal(page, result.Page);
        Assert.Equal(pageSize, result.Data.Count);
        Assert.Equal(3, result.TotalRecords);
    }
}
