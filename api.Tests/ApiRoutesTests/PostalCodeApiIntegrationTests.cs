using System.Net.Http.Json;
using api.Models;
using api.Utils;

namespace api.Tests.ApiRoutesTests;

public class PostalCodeApiIntegrationTests : IClassFixture<CustomWebApplicationFactory>
{
    private readonly HttpClient _client;

    public PostalCodeApiIntegrationTests(CustomWebApplicationFactory factory)
    {
        factory.ResetDatabase();
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task GetPostalCodes_ReturnsOkWithExpectedData()
    {
        var response = await _client.GetAsync("/api/v1/PostalCode");

        response.EnsureSuccessStatusCode();

        var result = await response.Content.ReadFromJsonAsync<List<PostalCode>>();

        Assert.NotNull(result);
        Assert.NotEmpty(result);
        Assert.Equal(3, result.Count);
    }

    [Fact]
    public async Task GetPostalCodeById_ReturnsOkWithData()
    {
        int id = 1;
        var response = await _client.GetAsync($"/api/v1/PostalCode/{id}");

        response.EnsureSuccessStatusCode();

        var result = await response.Content.ReadFromJsonAsync<PostalCode>();

        Assert.NotNull(result);
        Assert.Equal(id, result.Id);
        Assert.Equal("110111", result.Code);
    }

    [Fact]
    public async Task GetPostalCodeById_ReturnsBadRequest()
    {
        int id = 0;
        var response = await _client.GetAsync($"/api/v1/PostalCode/{id}");

        Assert.False(response.IsSuccessStatusCode);
        Assert.Equal(System.Net.HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task GetPostalCodeByCode_ReturnsOkWithExpectedData()
    {
        string code = "110111";
        var response = await _client.GetAsync($"/api/v1/PostalCode/code/{code}");

        response.EnsureSuccessStatusCode();

        var result = await response.Content.ReadFromJsonAsync<List<PostalCode>>();

        Assert.NotNull(result);
        Assert.NotEmpty(result);
        Assert.Single(result);
        Assert.Equal(code, result[0].Code);
    }

    [Fact]
    public async Task SearchPostalCodes_ReturnsOkWithFilteredResults()
    {
        string keyword = "rural";
        var response = await _client.GetAsync($"/api/v1/PostalCode/search/{keyword}");

        response.EnsureSuccessStatusCode();

        var result = await response.Content.ReadFromJsonAsync<List<PostalCode>>();

        Assert.NotNull(result);
        Assert.NotEmpty(result);
        Assert.Single(result);
        Assert.Contains(keyword, result[0].Type, StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public async Task GetPagedPostalCodes_ReturnsOkWithPagedData()
    {
        int page = 1;
        int pageSize = 2;

        var response = await _client.GetAsync($"/api/v1/PostalCode/pagedList?page={page}&pageSize={pageSize}");

        response.EnsureSuccessStatusCode();

        var result = await response.Content.ReadFromJsonAsync<PaginationResponseModel<PostalCode>>();

        Assert.NotNull(result);
        Assert.Equal(pageSize, result.PageSize);
        Assert.Equal(page, result.Page);
        Assert.Equal(pageSize, result.Data.Count);
        Assert.Equal(3, result.TotalRecords);
    }
}