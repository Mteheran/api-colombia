using System.Net.Http;
using System.Threading.Tasks;
using Xunit;
using Microsoft.AspNetCore.Mvc.Testing;

public class RegionApiIntegrationTests : IClassFixture<CustomWebApplicationFactory> , IDisposable
{
    private readonly HttpClient _client;

    public RegionApiIntegrationTests(CustomWebApplicationFactory factory) {
       _client = new CustomWebApplicationFactory().CreateClient(); 
    }

    public void Dispose()
    {
        _client.Dispose();
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
