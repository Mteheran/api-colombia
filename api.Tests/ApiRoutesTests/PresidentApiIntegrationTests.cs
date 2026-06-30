using System.Net.Http.Json;
using api.Models;
using api.Utils;

namespace api.Tests.ApiRoutesTests;

public class PresidentApiIntegrationTests : IClassFixture<CustomWebApplicationFactory>
{
    private readonly HttpClient _client;

    public PresidentApiIntegrationTests(CustomWebApplicationFactory factory)
    {
        factory.ResetDatabase();
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task GetPresidents_ReturnsOkWithExpectedData()
    {   
        var response = await _client.GetAsync("/api/v1/President");

        response.EnsureSuccessStatusCode();

        var result = await response.Content.ReadFromJsonAsync<List<President>>();

        Assert.NotNull(result);
        Assert.Equal(3, result.Count);
        Assert.Contains(result, p => p.Name == "Rafael");
    }

    [Fact]
    public async Task GetPresidents_WithValidSorting_ReturnsOrderedData()
    {
        var response = await _client.GetAsync("/api/v1/President?sortBy=name&sortDirection=desc");

        response.EnsureSuccessStatusCode();

        var result = await response.Content.ReadFromJsonAsync<List<President>>();

        Assert.NotNull(result);
        var expected = result.Select(p => p.Name).OrderByDescending(n => n).ToList();
        Assert.Equal(expected, result.Select(p => p.Name).ToList());
    }

    [Fact]
    public async Task GetPresidents_WithInvalidSorting_ReturnsBadRequest()
    {
        var response = await _client.GetAsync("/api/v1/President?sortBy=nonExistentColumn&sortDirection=asc");

        Assert.Equal(System.Net.HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task GetPresidentById_ReturnsOkWithPresidentData()
    {
        int presidentId = 1;  
        var response = await _client.GetAsync($"/api/v1/President/{presidentId}");

        response.EnsureSuccessStatusCode();

        var result = await response.Content.ReadFromJsonAsync<President>();
        
        Assert.NotNull(result);
        Assert.Equal(presidentId, result.Id);
    }

    [Fact]
    public async Task GetPresidentById_ReturnsBadRequest()
    {
        int presidentId = 0;  
        var response = await _client.GetAsync($"/api/v1/President/{presidentId}");

        Assert.False(response.IsSuccessStatusCode);
        Assert.Equal(System.Net.HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task GetPresidentById_ReturnsNotFound()
    {
        int presidentId = 9999;
        var response = await _client.GetAsync($"/api/v1/President/{presidentId}");

        Assert.Equal(System.Net.HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task GetPresidentByName_ReturnsOkWithPresidentData()
    {
        string presidentName = "Rafael";
        var response = await _client.GetAsync($"/api/v1/President/name/{presidentName}");

        response.EnsureSuccessStatusCode();

        var result = await response.Content.ReadFromJsonAsync<List<President>>();

        Assert.NotNull(result);
        Assert.Equal(presidentName, result[0].Name);
        Assert.NotNull(result[0].Description);
    }

    [Fact]
    public async Task GetPresidentByName_IsCaseInsensitive()
    {
        var response = await _client.GetAsync("/api/v1/President/name/rafael");

        response.EnsureSuccessStatusCode();

        var result = await response.Content.ReadFromJsonAsync<List<President>>();

        Assert.NotNull(result);
        Assert.Equal("Rafael", result[0].Name);
    }

    [Fact]
    public async Task GetPresidentByName_ReturnsNotFound()
    {
        var response = await _client.GetAsync("/api/v1/President/name/NonExistentName");

        Assert.Equal(System.Net.HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task GetPresidentsByYear_ReturnsOkWithFilteredData()
    {
        int year = 1865; 
        var response = await _client.GetAsync($"/api/v1/President/year/{year}");

        response.EnsureSuccessStatusCode();

        var result = await response.Content.ReadFromJsonAsync<List<President>>();
        
        Assert.NotNull(result);
        Assert.All(result, p => Assert.Equal(year, p.StartPeriodDate.Year));
    }

    [Fact]
    public async Task GetPresidentsByYear_ReturnsNotFound()
    {
        int year = 1700;
        var response = await _client.GetAsync($"/api/v1/President/year/{year}");

        Assert.Equal(System.Net.HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task GetPresidentsByYear_ReturnsBadRequest()
    {
        int year = 0;
        var response = await _client.GetAsync($"/api/v1/President/year/{year}");

        Assert.Equal(System.Net.HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task GetPresidentsByYear_IncludesPresidentWithoutEndPeriod()
    {
        int year = 1866;
        var response = await _client.GetAsync($"/api/v1/President/year/{year}");

        response.EnsureSuccessStatusCode();

        var result = await response.Content.ReadFromJsonAsync<List<President>>();

        Assert.NotNull(result);
        Assert.Contains(result, p => p.EndPeriodDate == null);
    }

    [Fact]
    public async Task SearchPresidents_ReturnsOkWithFilteredData()
    {
        string searchKeyword = "president";
        var response = await _client.GetAsync($"/api/v1/President/search/{searchKeyword}");

        response.EnsureSuccessStatusCode();

        var result = await response.Content.ReadFromJsonAsync<List<President>>();

        Assert.NotNull(result);
        Assert.Equal(2, result.Count);
        Assert.All(result, p => Assert.Contains(searchKeyword, p.Name, StringComparison.OrdinalIgnoreCase));
    }

    [Fact]
    public async Task SearchPresidents_ReturnsNotFound()
    {
        var response = await _client.GetAsync("/api/v1/President/search/zzzznomatch");

        Assert.Equal(System.Net.HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task GetPagedPresidents_ReturnsOkWithPagedData()
    {
        var pagination = new PaginationModel
        {
            Page = 1,
            PageSize = 2
        }; 
        var response = await _client.GetAsync($"/api/v1/President/pagedList?page={pagination.Page}&pageSize={pagination.PageSize}");

        response.EnsureSuccessStatusCode();

        var result = await response.Content.ReadFromJsonAsync<PaginationResponseModel<President>>();

        Assert.NotNull(result);
        Assert.Equal(pagination.Page, result.Page);
        Assert.Equal(pagination.PageSize, result.PageSize);
        Assert.Equal(3, result.TotalRecords);
    }

    [Fact]
    public async Task GetPagedPresidents_ReturnsBadRequest()
    {
        var response = await _client.GetAsync("/api/v1/President/pagedList?page=0&pageSize=0");

        Assert.Equal(System.Net.HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task GetPagedPresidents_PageOutOfRange_ReturnsNotFound()
    {
        var response = await _client.GetAsync("/api/v1/President/pagedList?page=999&pageSize=10");

        Assert.Equal(System.Net.HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task GetPagedPresidents_WithInvalidSorting_ReturnsBadRequest()
    {
        var response = await _client.GetAsync("/api/v1/President/pagedList?page=1&pageSize=2&sortBy=nonExistentColumn&sortDirection=asc");

        Assert.Equal(System.Net.HttpStatusCode.BadRequest, response.StatusCode);
    }
}