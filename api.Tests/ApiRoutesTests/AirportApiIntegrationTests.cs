using System.Net.Http;
using System.Threading.Tasks;
using Xunit;
using Microsoft.AspNetCore.Mvc.Testing;
using api.Models; 
using System.Net.Http.Json; 

public class AirportApiIntegrationTests : IClassFixture<CustomWebApplicationFactory>
{
    private readonly HttpClient _client;

    public AirportApiIntegrationTests(CustomWebApplicationFactory factory)
    {
        _client = factory.CreateClient();  
    }

    [Fact]
    public async Task GetAirports_ReturnsOkWithExpectedData()
    { 
        var response = await _client.GetAsync("/api/v1/Airport");
 
        response.EnsureSuccessStatusCode();  
         
       var result = await response.Content.ReadFromJsonAsync<List<Airport>>(); 

 
        Assert.NotNull(result);   
        Assert.Equal(5, result.Count);    
    }


    // [Fact]
    // public async Task GetAirportById_ReturnsOkWithAirportData()
    // {
    //     int airportId = 9;
    //     var response = await _client.GetAsync($"/api/v1/Airport/{airportId}");

    //     response.EnsureSuccessStatusCode();

    //     var result = await response.Content.ReadAsStringAsync();
        
    //     Assert.NotNull(result);
    //     Assert.Contains("Id", result);  
    // }

    // [Fact]
    // public async Task GetAirportByName_ReturnsOkWithAirportData()
    // {
    //     string airportName = "Rionegro";  
    //     var response = await _client.GetAsync($"/api/v1/Airport/name/{airportName}");

    //     response.EnsureSuccessStatusCode();

    //     var result = await response.Content.ReadAsStringAsync();
        
    //     Assert.NotNull(result);
    //     Assert.Contains("Rionegro", result);  
    // }

    // [Fact]
    // public async Task SearchAirports_ReturnsOkWithFilteredData()
    // {
    //     string searchKeyword = "Rionegro"; 
    //     var response = await _client.GetAsync($"/api/v1/Airport/search/{searchKeyword}");

    //     response.EnsureSuccessStatusCode();

    //     var result = await response.Content.ReadAsStringAsync();
        
    //     Assert.NotNull(result);
    //     Assert.Contains("Rionegro", result); 
    // }

    // [Fact]
    // public async Task GetPagedAirports_ReturnsOkWithPagedData()
    // {
    //     var pagination = new PaginationModel
    //     {
    //         Page = 1,
    //         PageSize = 10
    //     }; 
    //     var response = await _client.GetAsync($"/api/v1/Airport/pagedList?page={pagination.Page}&pageSize={pagination.PageSize}");

    //     response.EnsureSuccessStatusCode();

    //     var result = await response.Content.ReadAsStringAsync();

    //     Assert.NotNull(result);
    //     Assert.Contains("TotalRecords", result);  
    // }

}
