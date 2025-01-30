// using System.Net.Http;
// using System.Threading.Tasks;
// using Xunit;
// using Microsoft.AspNetCore.Mvc.Testing;

// public class TraditionalFairAndFestivalApiIntegrationTests : IClassFixture<CustomWebApplicationFactory>
// {
//     private readonly HttpClient _client;

//     public TraditionalFairAndFestivalApiIntegrationTests(CustomWebApplicationFactory factory)
//     {
//         _client = factory.CreateClient();
//     }

//     [Fact]
//     public async Task GetTraditionalFairAndFestivals_ReturnsOkWithExpectedData()
//     {
//         var response = await _client.GetAsync("/api/v1/TraditionalFairAndFestival");

//         response.EnsureSuccessStatusCode();

//         var result = await response.Content.ReadAsStringAsync();

//         Assert.NotNull(result);
//         Assert.False(string.IsNullOrEmpty(result));
//     }

//     [Fact]
//     public async Task GetTraditionalFairAndFestivalById_ReturnsOkWithData()
//     {
//         int id = 1; 
//         var response = await _client.GetAsync($"/api/v1/TraditionalFairAndFestival/{id}");

//         response.EnsureSuccessStatusCode();

//         var result = await response.Content.ReadAsStringAsync();

//         Assert.NotNull(result);
//         Assert.Contains("Id", result);
//     }

//     [Fact]
//     public async Task GetTraditionalFairAndFestivalByCity_ReturnsOkWithFilteredData()
//     {
//         int cityId = 1;  
//         var response = await _client.GetAsync($"/api/v1/TraditionalFairAndFestival/{cityId}/city");

//         response.EnsureSuccessStatusCode();

//         var result = await response.Content.ReadAsStringAsync();

//         Assert.NotNull(result);
//         Assert.Contains("City", result);
//     }

//     [Fact]
//     public async Task GetTraditionalFairAndFestivalByName_ReturnsOkWithExpectedData()
//     {
//         string name = "FestivalExample";  
//         var response = await _client.GetAsync($"/api/v1/TraditionalFairAndFestival/name/{name}");

//         response.EnsureSuccessStatusCode();

//         var result = await response.Content.ReadAsStringAsync();

//         Assert.NotNull(result);
//         Assert.Contains(name, result);
//     }

//     [Fact]
//     public async Task SearchTraditionalFairAndFestivals_ReturnsOkWithFilteredResults()
//     {
//         string keyword = "Example";  
//         var response = await _client.GetAsync($"/api/v1/TraditionalFairAndFestival/search/{keyword}");

//         response.EnsureSuccessStatusCode();

//         var result = await response.Content.ReadAsStringAsync();

//         Assert.NotNull(result);
//         Assert.Contains(keyword, result);
//     }

//     [Fact]
//     public async Task GetPagedTraditionalFairAndFestivals_ReturnsOkWithPagedData()
//     {
//         int page = 1;
//         int pageSize = 5;

//         var response = await _client.GetAsync($"/api/v1/TraditionalFairAndFestival/pagedList?page={page}&pageSize={pageSize}");

//         response.EnsureSuccessStatusCode();

//         var result = await response.Content.ReadAsStringAsync();

//         Assert.NotNull(result);
//         Assert.Contains("TotalRecords", result);
//     }
// }