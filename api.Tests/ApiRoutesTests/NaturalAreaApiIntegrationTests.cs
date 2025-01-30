// using System.Net.Http;
// using System.Threading.Tasks;
// using Xunit;
// using Microsoft.AspNetCore.Mvc.Testing;

// public class NaturalAreaApiIntegrationTests : IClassFixture<CustomWebApplicationFactory>
// {
//     private readonly HttpClient _client;

//     public NaturalAreaApiIntegrationTests(CustomWebApplicationFactory factory)
//     {
//         _client = factory.CreateClient();  
//     }

//     [Fact]
//     public async Task GetNaturalAreas_ReturnsOkWithExpectedData()
//     {
//         var response = await _client.GetAsync("/api/v1/NaturalArea/");
        
//         response.EnsureSuccessStatusCode();
        
//         var result = await response.Content.ReadAsStringAsync();
        
//         Assert.NotNull(result);    
//         Assert.False(string.IsNullOrEmpty(result));  
//     }

//     [Fact]
//     public async Task GetNaturalAreaById_ReturnsOkWithNaturalAreaData()
//     {
//         int naturalAreaId = 1;  
//         var response = await _client.GetAsync($"/api/v1/NaturalArea/{naturalAreaId}");

//         response.EnsureSuccessStatusCode();

//         var result = await response.Content.ReadAsStringAsync();
        
//         Assert.NotNull(result);
//         Assert.Contains("Id", result);  
//     }

//     [Fact]
//     public async Task GetNaturalAreaByName_ReturnsOkWithNaturalAreaData()
//     {
//         string naturalAreaName = "Sample Natural Area";  
//         var response = await _client.GetAsync($"/api/v1/NaturalArea/name/{naturalAreaName}");

//         response.EnsureSuccessStatusCode();

//         var result = await response.Content.ReadAsStringAsync();
        
//         Assert.NotNull(result);
//         Assert.Contains("Name", result);  
//     }

//     [Fact]
//     public async Task SearchNaturalAreas_ReturnsOkWithFilteredNaturalAreasData()
//     {
//         string keyword = "Sample";  
//         var response = await _client.GetAsync($"/api/v1/NaturalArea/search/{keyword}");

//         response.EnsureSuccessStatusCode();

//         var result = await response.Content.ReadAsStringAsync();
        
//         Assert.NotNull(result);
//         Assert.Contains("NaturalAreas", result);  
//     }

//     [Fact]
//     public async Task GetPagedNaturalAreas_ReturnsOkWithPagedNaturalAreasData()
//     {
//         string sortBy = "Name"; 
//         string sortDirection = "asc";  
//         int page = 1;
//         int pageSize = 10;
//         var response = await _client.GetAsync($"/api/v1/NaturalArea/pagedList?page={page}&pageSize={pageSize}&sortBy={sortBy}&sortDirection={sortDirection}");

//         response.EnsureSuccessStatusCode();

//         var result = await response.Content.ReadAsStringAsync();
        
//         Assert.NotNull(result);
//         Assert.Contains("Data", result);  
//     }

//     [Fact]
//     public async Task GetNaturalAreasWithSorting_ReturnsOkWithSortedData()
//     {
//         string sortBy = "Name"; 
//         string sortDirection = "asc";  

//         var response = await _client.GetAsync($"/api/v1/NaturalArea?sortBy={sortBy}&sortDirection={sortDirection}");

//         response.EnsureSuccessStatusCode();

//         var result = await response.Content.ReadAsStringAsync();
        
//         Assert.NotNull(result);
//         Assert.Contains("Name", result);  
//     }

//     [Fact]
//     public async Task GetNaturalAreaById_ReturnsNotFoundWhenIdIsInvalid()
//     {
//         int invalidId = -1;  
//         var response = await _client.GetAsync($"/api/v1/NaturalArea/{invalidId}");

//         Assert.Equal(System.Net.HttpStatusCode.BadRequest, response.StatusCode);
//     }
// }
