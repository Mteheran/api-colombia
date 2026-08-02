using System.Net.Http.Json;
using api.Models;
using api.Utils;

namespace api.Tests.ApiRoutesTests;

public class HigherEducationInstitutionApiIntegrationTests : IClassFixture<CustomWebApplicationFactory>
{
    private readonly HttpClient _client;

    public HigherEducationInstitutionApiIntegrationTests(CustomWebApplicationFactory factory)
    {
        factory.ResetDatabase();
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task GetHigherEducationInstitutions_ReturnsOkWithExpectedData()
    {
        var response = await _client.GetAsync("/api/v1/HigherEducationInstitution");

        response.EnsureSuccessStatusCode();

        var result = await response.Content.ReadFromJsonAsync<List<HigherEducationInstitution>>();

        Assert.NotNull(result);
        Assert.NotEmpty(result);
        Assert.Equal(3, result.Count);
    }

    [Fact]
    public async Task GetHigherEducationInstitutionById_ReturnsOkWithData()
    {
        int id = 1;
        var response = await _client.GetAsync($"/api/v1/HigherEducationInstitution/{id}");

        response.EnsureSuccessStatusCode();

        var result = await response.Content.ReadFromJsonAsync<HigherEducationInstitution>();

        Assert.NotNull(result);
        Assert.Equal(id, result.Id);
        Assert.Equal("1101", result.Code);
        Assert.True(result.IsHighQualityAccredited);
    }

    [Fact]
    public async Task GetHigherEducationInstitutionById_ReturnsBadRequest()
    {
        int id = 0;
        var response = await _client.GetAsync($"/api/v1/HigherEducationInstitution/{id}");

        Assert.False(response.IsSuccessStatusCode);
        Assert.Equal(System.Net.HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task GetHigherEducationInstitutionById_ReturnsNotFound()
    {
        int id = 9999;
        var response = await _client.GetAsync($"/api/v1/HigherEducationInstitution/{id}");

        Assert.Equal(System.Net.HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task GetHigherEducationInstitutionByName_ReturnsOkWithExpectedData()
    {
        string name = "Universidad del Valle";
        var response = await _client.GetAsync($"/api/v1/HigherEducationInstitution/name/{name}");

        response.EnsureSuccessStatusCode();

        var result = await response.Content.ReadFromJsonAsync<List<HigherEducationInstitution>>();

        Assert.NotNull(result);
        Assert.Single(result);
        Assert.Equal("Universidad del Valle", result[0].Name);
    }

    [Fact]
    public async Task SearchHigherEducationInstitutions_ReturnsOkWithFilteredResults()
    {
        string keyword = "Pascual";
        var response = await _client.GetAsync($"/api/v1/HigherEducationInstitution/search/{keyword}");

        response.EnsureSuccessStatusCode();

        var result = await response.Content.ReadFromJsonAsync<List<HigherEducationInstitution>>();

        Assert.NotNull(result);
        Assert.Single(result);
        Assert.Contains(keyword, result[0].Name, StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public async Task GetPagedHigherEducationInstitutions_ReturnsOkWithPagedData()
    {
        int page = 1;
        int pageSize = 2;

        var response = await _client.GetAsync($"/api/v1/HigherEducationInstitution/pagedList?page={page}&pageSize={pageSize}");

        response.EnsureSuccessStatusCode();

        var result = await response.Content.ReadFromJsonAsync<PaginationResponseModel<HigherEducationInstitution>>();

        Assert.NotNull(result);
        Assert.Equal(pageSize, result.PageSize);
        Assert.Equal(page, result.Page);
        Assert.Equal(pageSize, result.Data.Count);
        Assert.Equal(3, result.TotalRecords);
    }

    [Fact]
    public async Task GetHigherEducationInstitutionsByCity_ReturnsOkWithExpectedData()
    {
        int cityId = 1;
        var response = await _client.GetAsync($"/api/v1/City/{cityId}/highereducationinstitutions");

        response.EnsureSuccessStatusCode();

        var result = await response.Content.ReadFromJsonAsync<List<HigherEducationInstitution>>();

        Assert.NotNull(result);
        Assert.Equal(2, result.Count);
        Assert.All(result, i => Assert.Equal(cityId, i.CityId));
    }
}
