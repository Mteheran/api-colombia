// using System.Net.Http;
// using System.Threading.Tasks;
// using Xunit;
// using Microsoft.AspNetCore.Mvc.Testing;
// using api.Models;

// public class TouristAttractionApiIntegrationTests : IClassFixture<CustomWebApplicationFactory>
// {
//     private readonly HttpClient _client;

//     public TouristAttractionApiIntegrationTests(CustomWebApplicationFactory factory)
//     {
//         _client = factory.CreateClient();
//     }

//     [Fact]
//     public async Task GetTouristAttractions_ReturnsOkWithExpectedData()
//     {
//         var response = await _client.GetAsync("/api/v1/TouristAttraction");

//         response.EnsureSuccessStatusCode();

//         var result = await response.Content.ReadAsStringAsync();

//         Assert.NotNull(result);
//         Assert.False(string.IsNullOrEmpty(result));
//     }

//     [Fact]
//     public async Task GetTouristAttractionById_ReturnsOkWithTouristAttractionData()
//     {
//         int attractionId = 1;  
//         var response = await _client.GetAsync($"/api/v1/TouristAttraction/{attractionId}");

//         response.EnsureSuccessStatusCode();

//         var result = await response.Content.ReadAsStringAsync();

//         Assert.NotNull(result);
//         Assert.Contains("Id", result);
//     }

//     [Fact]
//     public async Task GetTouristAttractionByName_ReturnsOkWithTouristAttractionData()
//     {
//         string attractionName = "TestAttraction";  
//         var response = await _client.GetAsync($"/api/v1/TouristAttraction/name/{attractionName}");

//         response.EnsureSuccessStatusCode();

//         var result = await response.Content.ReadAsStringAsync();

//         Assert.NotNull(result);
//         Assert.Contains("TestAttraction", result);
//     }

//     [Fact]
//     public async Task SearchTouristAttractions_ReturnsOkWithFilteredData()
//     {
//         string searchKeyword = "Test";  
//         var response = await _client.GetAsync($"/api/v1/TouristAttraction/search/{searchKeyword}");

//         response.EnsureSuccessStatusCode();

//         var result = await response.Content.ReadAsStringAsync();

//         Assert.NotNull(result);
//         Assert.Contains("Test", result);
//     }

//     [Fact]
//     public async Task GetPagedTouristAttractions_ReturnsOkWithPagedData()
//     {
//         int page = 1;
//         int pageSize = 10;

//         var response = await _client.GetAsync($"/api/v1/TouristAttraction/pagedList?page={page}&pageSize={pageSize}");

//         response.EnsureSuccessStatusCode();

//         var result = await response.Content.ReadAsStringAsync();

//         Assert.NotNull(result);
//         Assert.Contains("TotalRecords", result);
//     }
// }
