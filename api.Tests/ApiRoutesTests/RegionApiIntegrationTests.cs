namespace api.Tests.ApiRoutesTests;

public class RegionApiIntegrationTests : IClassFixture<CustomWebApplicationFactory>
{
    private readonly HttpClient _client;

    public RegionApiIntegrationTests(CustomWebApplicationFactory factory)
    {
        factory.ResetDatabase();
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task GetRegions_ReturnsOkWithExpectedData()
    {
        var response = await _client.GetAsync("/api/v1/Region");

        response.EnsureSuccessStatusCode();

        var result = await response.Content.ReadAsStringAsync();

        Assert.NotNull(result);
        Assert.False(string.IsNullOrEmpty(result));
    }

    [Fact]
    public async Task GetRegions_InvalidSortBy_ReturnsBadRequest()
    {
        var response = await _client.GetAsync("/api/v1/Region?sortBy=Invalid&sortDirection=asc");
        Assert.Equal(System.Net.HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task GetRegions_InvalidSortDirection_ReturnsBadRequest()
    {
        var response = await _client.GetAsync("/api/v1/Region?sortBy=Name&sortDirection=invalid");
        Assert.Equal(System.Net.HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task GetRegionById_ReturnsOkWithRegionData()
    {
        int regionId = 1;  
        var response = await _client.GetAsync($"/api/v1/Region/{regionId}");

        response.EnsureSuccessStatusCode();

        var result = await response.Content.ReadAsStringAsync();

        Assert.NotNull(result);
        Assert.Contains("id", result);
    }

    [Fact]
    public async Task GetRegionById_ReturnsOkWithBadRequest()
    {
        int regionId = 0;  
        var response = await _client.GetAsync($"/api/v1/Region/{regionId}");

        Assert.False(response.IsSuccessStatusCode);
        Assert.Equal(System.Net.HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task GetRegionDepartmentsById_ReturnsOkWithDepartmentsData()
    {
        int regionId = 1;  
        var response = await _client.GetAsync($"/api/v1/Region/{regionId}/departments");

        response.EnsureSuccessStatusCode();

        var result = await response.Content.ReadAsStringAsync();

        Assert.NotNull(result);
        Assert.Contains("departments", result);
    }

    [Fact]
    public async Task GetRegionDepartments_InvalidSortBy_ReturnsBadRequest()
    {
        var response = await _client.GetAsync("/api/v1/Region/1/departments?sortBy=Invalid&sortDirection=asc");
        Assert.Equal(System.Net.HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task GetRegionDepartments_InvalidSortDirection_ReturnsBadRequest()
    {
        var response = await _client.GetAsync("/api/v1/Region/1/departments?sortBy=Name&sortDirection=invalid");
        Assert.Equal(System.Net.HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task GetSortedRegions_ReturnsOkWithSortedData()
    {
        string sortBy = "name";  
        string sortDirection = "asc";  
        var response = await _client.GetAsync($"/api/v1/Region?sortBy={sortBy}&sortDirection={sortDirection}");

        response.EnsureSuccessStatusCode();

        var result = await response.Content.ReadAsStringAsync();

        Assert.NotNull(result);
        Assert.False(string.IsNullOrEmpty(result));
    }

    [Fact]
    public async Task GetInvalidRegionId_ReturnsNotFound()
    {
        int invalidRegionId = 1000;  
        var response = await _client.GetAsync($"/api/v1/Region/{invalidRegionId}");

        Assert.Equal(System.Net.HttpStatusCode.NotFound, response.StatusCode);
    }
}