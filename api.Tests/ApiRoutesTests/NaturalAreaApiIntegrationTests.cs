using System.Net.Http;
using System.Threading.Tasks;
using Xunit;
using Microsoft.AspNetCore.Mvc.Testing;

public class NaturalAreaApiIntegrationTests : IClassFixture<CustomWebApplicationFactory> , IDisposable
{
    private readonly HttpClient _client;

    public NaturalAreaApiIntegrationTests(CustomWebApplicationFactory factory)
    {
       _client = new CustomWebApplicationFactory().CreateClient(); 
    }

    public void Dispose()
    {
        _client.Dispose();
    }

    [Fact]
    public async Task GetNaturalAreas_ReturnsOkWithExpectedData()
    {
        var response = await _client.GetAsync("/api/v1/NaturalArea/");
        
        response.EnsureSuccessStatusCode();
        
        var result = await response.Content.ReadAsStringAsync();
        
        Assert.NotNull(result);    
        Assert.False(string.IsNullOrEmpty(result));  
    }

    [Fact]
    public async Task GetNaturalAreaById_ReturnsOkWithNaturalAreaData()
    {
        int naturalAreaId = 1;  
        var response = await _client.GetAsync($"/api/v1/NaturalArea/{naturalAreaId}");

        response.EnsureSuccessStatusCode();

        var result = await response.Content.ReadAsStringAsync();
        
        Assert.NotNull(result);
        Assert.Contains("id", result);  
    }

    [Fact]
    public async Task GetNaturalAreaByName_ReturnsOkWithNaturalAreaData()
    {
        string naturalAreaName = "Parque Arví";  
        var response = await _client.GetAsync($"/api/v1/NaturalArea/name/{naturalAreaName}");

        response.EnsureSuccessStatusCode();

        var result = await response.Content.ReadAsStringAsync();
        
        Assert.NotNull(result);
        Assert.Contains("name", result);  
    }

    [Fact]
    public async Task SearchNaturalAreas_ReturnsOkWithFilteredNaturalAreasData()
    {
        string keyword = "Parque";  
        var response = await _client.GetAsync($"/api/v1/NaturalArea/search/{keyword}");

        response.EnsureSuccessStatusCode();

        var result = await response.Content.ReadAsStringAsync();
        
        Assert.NotNull(result);
        Assert.Contains("categoryNatural", result);  
    }

    [Fact]
    public async Task GetPagedNaturalAreas_ReturnsOkWithPagedNaturalAreasData()
    {
        string sortBy = "Name"; 
        string sortDirection = "asc";  
        int page = 1;
        int pageSize = 10;
        var response = await _client.GetAsync($"/api/v1/NaturalArea/pagedList?page={page}&pageSize={pageSize}&sortBy={sortBy}&sortDirection={sortDirection}");

        response.EnsureSuccessStatusCode();

        var result = await response.Content.ReadAsStringAsync();
        
        Assert.NotNull(result);
        Assert.Contains("pageSize", result);  
    }

    [Fact]
    public async Task GetNaturalAreasWithSorting_ReturnsOkWithSortedData()
    {
        string sortBy = "id"; 
        string sortDirection = "asc";  

        var response = await _client.GetAsync($"/api/v1/NaturalArea?sortBy={sortBy}&sortDirection={sortDirection}");

        response.EnsureSuccessStatusCode();

        var result = await response.Content.ReadAsStringAsync();
        
        Assert.NotNull(result);
        Assert.Contains("id", result);  
    }

    [Fact]
    public async Task GetNaturalAreaById_ReturnsNotFoundWhenIdIsInvalid()
    {
        int invalidId = -1;  
        var response = await _client.GetAsync($"/api/v1/NaturalArea/{invalidId}");

        Assert.Equal(System.Net.HttpStatusCode.BadRequest, response.StatusCode);
    }
}
