// using System.Net.Http;
// using System.Threading.Tasks;
// using Xunit;
// using Microsoft.AspNetCore.Mvc.Testing;

// public class MapsApiIntegrationTests : IClassFixture<CustomWebApplicationFactory>
// {
//     private readonly HttpClient _client;

//     public MapsApiIntegrationTests(CustomWebApplicationFactory factory)
//     {
//         _client = factory.CreateClient();  
//     }

//     [Fact]
//     public async Task GetMaps_ReturnsOkWithExpectedData()
//     { 
//         string sortBy = "Name";  
//         string sortDirection = "asc";  
 
//         var response = await _client.GetAsync($"/api/v1/Maps?sortBy={sortBy}&sortDirection={sortDirection}");
 
//         response.EnsureSuccessStatusCode();

//         var result = await response.Content.ReadAsStringAsync();

//         Assert.NotNull(result);    
//         Assert.False(string.IsNullOrEmpty(result));  
//     }

//     [Fact]
//     public async Task GetMapsWithInvalidSorting_ReturnsBadRequest()
//     { 
//         string sortBy = "InvalidField";  
//         string sortDirection = "asc";  
 
//         var response = await _client.GetAsync($"/api/v1/Maps?sortBy={sortBy}&sortDirection={sortDirection}");
 
//         Assert.Equal(System.Net.HttpStatusCode.BadRequest, response.StatusCode);  
//     }
// }
