using System.Net.Http.Json;
using api.Models;
using api.Utils;

namespace api.Tests.ApiRoutesTests;

public class AirportApiIntegrationTests : IClassFixture<CustomWebApplicationFactory>
{
    private readonly HttpClient _client;

    public AirportApiIntegrationTests(CustomWebApplicationFactory factory)
    {
        factory.ResetDatabase();
        _client = factory.CreateClient();  
    }

    [Fact]
    public async Task GetAirports_ReturnsOkWithExpectedData()
    { 
        var response = await _client.GetAsync("/api/v1/Airport");
 
        response.EnsureSuccessStatusCode();  
         
        var result = await response.Content.ReadFromJsonAsync<List<Airport>>();

        Assert.NotNull(result);   
        Assert.Single(result);    
    }


    [Fact]
    public async Task GetAirportById_ReturnsOkWithAirportData()
    {
        int airportId = 1;
        var response = await _client.GetAsync($"/api/v1/Airport/{airportId}");

        response.EnsureSuccessStatusCode();

        var result = await response.Content.ReadFromJsonAsync<Airport>();
        
        Assert.NotNull(result);
        Assert.Equal(airportId, result.Id);  
    }

    [Fact]
    public async Task GetAirportById_ReturnsBadRequest()
    {
        int airportId = 0;
        var response = await _client.GetAsync($"/api/v1/Airport/{airportId}");
        
        Assert.False(response.IsSuccessStatusCode);
        Assert.Equal(System.Net.HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task GetAirportByName_ReturnsOkWithAirportData()
    {
        string airportName = "Rionegro";  
        var response = await _client.GetAsync($"/api/v1/Airport/name/{airportName}");

        response.EnsureSuccessStatusCode();

        var result = await response.Content.ReadFromJsonAsync<List<Airport>>();
        
        Assert.NotNull(result);
        Assert.Equal("Rionegro", result[0].Name);  
    }

    [Fact]
    public async Task SearchAirports_ReturnsOkWithFilteredData()
    {
        string searchKeyword = "rione"; 
        var response = await _client.GetAsync($"/api/v1/Airport/search/{searchKeyword}");

        response.EnsureSuccessStatusCode();

        var result = await response.Content.ReadFromJsonAsync<List<Airport>>();
        
        Assert.NotNull(result);
        Assert.Single(result); 
        Assert.Equal("Rionegro", result[0].Name);
    }

    [Fact]
    public async Task GetPagedAirports_ReturnsOkWithPagedData()
    {
        var paginationParams = "page=1&pageSize=10";
        var response = await _client.GetAsync($"/api/v1/Airport/pagedList?{paginationParams}");

        response.EnsureSuccessStatusCode();

        var result = await response.Content.ReadFromJsonAsync<PaginationResponseModel<Airport>>();

        Assert.NotNull(result);
        Assert.Single(result.Data);  
    }

}
