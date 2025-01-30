// using System.Net.Http;
// using System.Threading.Tasks;
// using Xunit;
// using Microsoft.AspNetCore.Mvc.Testing;

// public class CityApiIntegrationTests : IClassFixture<CustomWebApplicationFactory>
// {
//     private readonly HttpClient _client;

//     public CityApiIntegrationTests(CustomWebApplicationFactory factory)
//     {
//         _client = factory.CreateClient();
//     }

//     [Fact]
//     public async Task GetCities_ReturnsOkWithExpectedData()
//     {   
//         var response = await _client.GetAsync("/api/v1/City");

//         response.EnsureSuccessStatusCode();

//         var result = await response.Content.ReadAsStringAsync();

//         Assert.NotNull(result);
//         Assert.False(string.IsNullOrEmpty(result));  
//     }

//     [Fact]
//     public async Task GetCityById_ReturnsOkWithCityData()
//     {
//         int cityId = 1;  
//         var response = await _client.GetAsync($"/api/v1/City/{cityId}");

//         response.EnsureSuccessStatusCode();

//         var result = await response.Content.ReadAsStringAsync();

//         Assert.NotNull(result);
//         Assert.Contains("Id", result);
//     }

//     [Fact]
//     public async Task GetCityByName_ReturnsOkWithCityData()
//     {
//         string cityName = "Sample City";  
//         var response = await _client.GetAsync($"/api/v1/City/name/{cityName}");

//         response.EnsureSuccessStatusCode();

//         var result = await response.Content.ReadAsStringAsync();

//         Assert.NotNull(result);
//         Assert.Contains("Name", result);
//     }

//     [Fact]
//     public async Task SearchCities_ReturnsOkWithFilteredData()
//     {
//         string keyword = "Sample";  
//         var response = await _client.GetAsync($"/api/v1/City/search/{keyword}");

//         response.EnsureSuccessStatusCode();

//         var result = await response.Content.ReadAsStringAsync();

//         Assert.NotNull(result);
//         Assert.Contains("Sample", result);
//     }

//     [Fact]
//     public async Task GetCitiesPagedList_ReturnsOkWithPagedData()
//     {
//         var paginationParams = "?page=1&pageSize=10";
//         var response = await _client.GetAsync($"/api/v1/City/pagedList{paginationParams}");

//         response.EnsureSuccessStatusCode();

//         var result = await response.Content.ReadAsStringAsync();

//         Assert.NotNull(result);
//         Assert.Contains("Page", result);
//         Assert.Contains("PageSize", result);
//     }

//     [Fact]
//     public async Task GetCitiesWithSorting_ReturnsOkWithSortedData()
//     {
//         string sortBy = "Name"; 
//         string sortDirection = "asc";  

//         var response = await _client.GetAsync($"/api/v1/City?sortBy={sortBy}&sortDirection={sortDirection}");

//         response.EnsureSuccessStatusCode();

//         var result = await response.Content.ReadAsStringAsync();

//         Assert.NotNull(result);
//         Assert.Contains("Name", result);  
//     }
// }
