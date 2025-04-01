using System.Net.Http.Json;
using api.Models;

public class CategoryNaturalAreaApiIntegrationTests : IClassFixture<CustomWebApplicationFactory> , IDisposable
{
    private readonly HttpClient _client;

    public CategoryNaturalAreaApiIntegrationTests()
    {
       _client = new CustomWebApplicationFactory().CreateClient(); 
    }

    public void Dispose()
    {
        _client.Dispose();
    }

    [Fact]
    public async Task GetCategoryNaturalAreas_ReturnsOkWithExpectedData()
    { 
        var response = await _client.GetAsync("/api/v1/CategoryNaturalArea");
 
        response.EnsureSuccessStatusCode();  
        
        var result = await response.Content.ReadFromJsonAsync<List<CategoryNaturalArea>>(); 
        
        Assert.NotNull(result);   
        Assert.Equal(3, result.Count);
    }


    [Fact]
    public async Task GetCategoryNaturalArea_ById_ReturnsOkWithExpectedData()
    { 
        var expectedId = 1;

        var response = await _client.GetAsync($"/api/v1/CategoryNaturalArea/{expectedId}");
        response.EnsureSuccessStatusCode();  

        var result = await response.Content.ReadFromJsonAsync<CategoryNaturalArea>(); 
  
        Assert.NotNull(result);  
        Assert.Equal(expectedId, result.Id);
    }

    [Fact]
    public async Task GetCategoryNaturalArea_ById_ReturnsBadRequest_ForInvalidId()
    { 
        var invalidId = -1;  
 
        var response = await _client.GetAsync($"/api/v1/CategoryNaturalArea/{invalidId}");
 
        Assert.Equal(System.Net.HttpStatusCode.BadRequest, response.StatusCode); 
    }


    [Fact]
    public async Task GetCategoryNaturalAreaWithNaturalAreas_ReturnsOkWithCategoryAndNaturalAreasData()
    { 
        int categoryNaturalAreaId = 1;  
        var response = await _client.GetAsync($"/api/v1/CategoryNaturalArea/{categoryNaturalAreaId}/NaturalAreas");

        response.EnsureSuccessStatusCode();

        var result = await response.Content.ReadFromJsonAsync<CategoryNaturalArea>(); 
        
        Assert.NotNull(result);
        Assert.Single(result.NaturalAreas); 
    }

    [Fact]
    public async Task GetCategoryNaturalAreasWithSorting_ReturnsOkWithSortedData()
    {
        string sortBy = "id"; 
        string sortDirection = "desc";  

        var response = await _client.GetAsync($"/api/v1/CategoryNaturalArea?sortBy={sortBy}&sortDirection={sortDirection}");

        response.EnsureSuccessStatusCode();

        var result = await response.Content.ReadFromJsonAsync<List<CategoryNaturalArea>>(); 
        
        Assert.NotNull(result);
        Assert.Equal(3, result.Count);
        Assert.Equal(3, result[0].Id);  
    }
}



