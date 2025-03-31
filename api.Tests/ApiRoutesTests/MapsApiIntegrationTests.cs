using api.Models;
using System.Net.Http.Json;

public class MapsApiIntegrationTests : IClassFixture<CustomWebApplicationFactory>
{
    private readonly HttpClient _client;

    public MapsApiIntegrationTests(CustomWebApplicationFactory factory)
    {
        _client = factory.CreateClient();  
    }

    [Fact]
    public async Task GetMaps_ReturnsOkWithExpectedData()
    { 
        string sortBy = "Name";  
        string sortDirection = "asc";  
 
        var response = await _client.GetAsync($"/api/v1/Map?sortBy={sortBy}&sortDirection={sortDirection}");
 
        response.EnsureSuccessStatusCode();

       var result = await response.Content.ReadFromJsonAsync<List<Map>>(); 

        Assert.NotNull(result);
        Assert.Equal(2, result.Count);
        Assert.Equal("Mapa de Colombia", result[0].Name);
    }

    [Fact]
    public async Task GetMapsWithInvalidSorting_ReturnsBadRequest()
    { 
        string sortBy = "InvalidField";  
        string sortDirection = "asc";  
 
        var response = await _client.GetAsync($"/api/v1/Map?sortBy={sortBy}&sortDirection={sortDirection}");

        Assert.Equal(System.Net.HttpStatusCode.BadRequest, response.StatusCode);  
    }

    
    [Fact]
    public async Task GetMapById_ReturnsOk()
    {
        int itemId = 1;
        var response = await _client.GetAsync($"/api/v1/Map/{itemId}");

        response.EnsureSuccessStatusCode();

        var result = await response.Content.ReadFromJsonAsync<Map>();
        
        Assert.NotNull(result);
        Assert.Equal(itemId, result.Id);
    }

    [Fact]
    public async Task GetMapById_ReturnsBadRequest()
    {
        int itemId = 0;
        var response = await _client.GetAsync($"/api/v1/Map/{itemId}");

        Assert.False(response.IsSuccessStatusCode);
        Assert.Equal(System.Net.HttpStatusCode.BadRequest, response.StatusCode);
    }
}
