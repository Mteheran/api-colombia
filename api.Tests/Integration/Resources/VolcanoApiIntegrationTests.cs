using System.Net.Http.Json;
using api.Models;
using api.Utils;

using api.Tests.Infrastructure;

namespace api.Tests.Integration.Resources;

[Collection(SharedApiCollection.Name)]
public class VolcanoApiIntegrationTests(ApiColombiaFactory factory) : IntegrationTestBase(factory)
{
    [Fact]
    public async Task GetVolcanoes_ReturnsOkWithExpectedData()
    {
        var response = await _client.GetAsync("/api/v1/Volcano");

        response.EnsureSuccessStatusCode();

        var result = await response.Content.ReadFromJsonAsync<List<Volcano>>();

        Assert.NotNull(result);
        Assert.NotEmpty(result);
        Assert.Equal(3, result.Count);
    }

    [Fact]
    public async Task GetVolcanoes_WithInvalidSort_ReturnsBadRequest()
    {
        var response = await _client.GetAsync("/api/v1/Volcano?sortBy=NotAField&sortDirection=asc");

        Assert.Equal(System.Net.HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task GetVolcanoes_SortedByElevation_ReturnsOrderedData()
    {
        var response = await _client.GetAsync("/api/v1/Volcano?sortBy=Elevation&sortDirection=desc");

        response.EnsureSuccessStatusCode();

        var result = await response.Content.ReadFromJsonAsync<List<Volcano>>();

        Assert.NotNull(result);
        Assert.Equal(new[] { 5321, 4276, 2750 }, result.Select(v => v.Elevation));
    }

    [Fact]
    public async Task GetVolcanoById_ReturnsOkWithData()
    {
        int id = 1;
        var response = await _client.GetAsync($"/api/v1/Volcano/{id}");

        response.EnsureSuccessStatusCode();

        var result = await response.Content.ReadFromJsonAsync<Volcano>();

        Assert.NotNull(result);
        Assert.Equal(id, result.Id);
        Assert.Equal("Volcán Nevado del Ruiz", result.Name);
        Assert.Equal(5321, result.Elevation);
        Assert.Equal("Alerta amarilla", result.ActivityLevel);
        Assert.NotNull(result.Department);
        Assert.NotNull(result.City);
    }

    [Fact]
    public async Task GetVolcanoById_ReturnsBadRequest()
    {
        int id = 0;
        var response = await _client.GetAsync($"/api/v1/Volcano/{id}");

        Assert.False(response.IsSuccessStatusCode);
        Assert.Equal(System.Net.HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task GetVolcanoById_ReturnsNotFound()
    {
        int id = 9999;
        var response = await _client.GetAsync($"/api/v1/Volcano/{id}");

        Assert.Equal(System.Net.HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task GetVolcanoByName_ReturnsOkWithExpectedData()
    {
        string name = "Galeras";
        var response = await _client.GetAsync($"/api/v1/Volcano/name/{name}");

        response.EnsureSuccessStatusCode();

        var result = await response.Content.ReadFromJsonAsync<List<Volcano>>();

        Assert.NotNull(result);
        Assert.Single(result);
        Assert.Equal("Volcán Galeras", result[0].Name);
    }

    [Fact]
    public async Task SearchVolcanoes_ReturnsOkWithFilteredResults()
    {
        string keyword = "Machin";
        var response = await _client.GetAsync($"/api/v1/Volcano/search/{keyword}");

        response.EnsureSuccessStatusCode();

        var result = await response.Content.ReadFromJsonAsync<List<Volcano>>();

        Assert.NotNull(result);
        Assert.Single(result);
        Assert.Equal("Volcán Cerro Machín", result[0].Name);
    }

    [Fact]
    public async Task SearchVolcanoes_ByVolcanoType_ReturnsOkWithFilteredResults()
    {
        string keyword = "Estratovolcan";
        var response = await _client.GetAsync($"/api/v1/Volcano/search/{keyword}");

        response.EnsureSuccessStatusCode();

        var result = await response.Content.ReadFromJsonAsync<List<Volcano>>();

        Assert.NotNull(result);
        Assert.Equal(2, result.Count);
    }

    [Fact]
    public async Task SearchVolcanoes_WithoutMatches_ReturnsEmptyList()
    {
        string keyword = "NotAVolcanoName";
        var response = await _client.GetAsync($"/api/v1/Volcano/search/{keyword}");

        response.EnsureSuccessStatusCode();

        var result = await response.Content.ReadFromJsonAsync<List<Volcano>>();

        Assert.NotNull(result);
        Assert.Empty(result);
    }

    [Fact]
    public async Task GetPagedVolcanoes_ReturnsOkWithPagedData()
    {
        int page = 1;
        int pageSize = 2;

        var response = await _client.GetAsync($"/api/v1/Volcano/pagedList?page={page}&pageSize={pageSize}");

        response.EnsureSuccessStatusCode();

        var result = await response.Content.ReadFromJsonAsync<PaginationResponseModel<Volcano>>();

        Assert.NotNull(result);
        Assert.Equal(pageSize, result.PageSize);
        Assert.Equal(page, result.Page);
        Assert.Equal(pageSize, result.Data.Count);
        Assert.Equal(3, result.TotalRecords);
    }

    [Fact]
    public async Task GetPagedVolcanoes_WithInvalidPage_ReturnsBadRequest()
    {
        var response = await _client.GetAsync("/api/v1/Volcano/pagedList?page=0&pageSize=10");

        Assert.Equal(System.Net.HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task GetVolcanoesByDepartment_ReturnsOkWithExpectedData()
    {
        int departmentId = 1;
        var response = await _client.GetAsync($"/api/v1/Department/{departmentId}/volcanoes");

        response.EnsureSuccessStatusCode();

        var result = await response.Content.ReadFromJsonAsync<List<Volcano>>();

        Assert.NotNull(result);
        Assert.Equal(2, result.Count);
        Assert.All(result, v => Assert.Equal(departmentId, v.DepartmentId));
    }

    [Fact]
    public async Task GetVolcanoesByDepartment_ReturnsBadRequest()
    {
        var response = await _client.GetAsync("/api/v1/Department/0/volcanoes");

        Assert.Equal(System.Net.HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task GetVolcanoesByCity_ReturnsOkWithExpectedData()
    {
        int cityId = 1;
        var response = await _client.GetAsync($"/api/v1/City/{cityId}/volcanoes");

        response.EnsureSuccessStatusCode();

        var result = await response.Content.ReadFromJsonAsync<List<Volcano>>();

        Assert.NotNull(result);
        Assert.Equal(2, result.Count);
        Assert.All(result, v => Assert.Equal(cityId, v.CityId));
    }

    [Fact]
    public async Task GetVolcanoesByCity_ReturnsBadRequest()
    {
        var response = await _client.GetAsync("/api/v1/City/0/volcanoes");

        Assert.Equal(System.Net.HttpStatusCode.BadRequest, response.StatusCode);
    }
}
