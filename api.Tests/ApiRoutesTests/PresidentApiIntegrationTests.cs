// using System.Net.Http;
// using System.Threading.Tasks;
// using Xunit;
// using Microsoft.AspNetCore.Mvc.Testing;
// using System.Collections.Generic;
// using api.Models;

// public class PresidentApiIntegrationTests : IClassFixture<CustomWebApplicationFactory>
// {
//     private readonly HttpClient _client;

//     public PresidentApiIntegrationTests(CustomWebApplicationFactory factory)
//     {
//         _client = factory.CreateClient();  
//     }

//     [Fact]
//     public async Task GetPresidents_ReturnsOkWithExpectedData()
//     {   
//         var response = await _client.GetAsync("/api/v1/President");

//         response.EnsureSuccessStatusCode();

//         var result = await response.Content.ReadAsStringAsync();

//         Assert.NotNull(result);    
//         Assert.False(string.IsNullOrEmpty(result));  
//     }

//     [Fact]
//     public async Task GetPresidentById_ReturnsOkWithPresidentData()
//     {
//         int presidentId = 1;  
//         var response = await _client.GetAsync($"/api/v1/President/{presidentId}");

//         response.EnsureSuccessStatusCode();

//         var result = await response.Content.ReadAsStringAsync();
        
//         Assert.NotNull(result);
//         Assert.Contains("Id", result);  
//     }

//     [Fact]
//     public async Task GetPresidentByName_ReturnsOkWithPresidentData()
//     {
//         string presidentName = "Rafael";  
//         var response = await _client.GetAsync($"/api/v1/President/name/{presidentName}");

//         response.EnsureSuccessStatusCode();

//         var result = await response.Content.ReadAsStringAsync();
        
//         Assert.NotNull(result);
//         Assert.Contains("Rafael", result);  
//     }

//     [Fact]
//     public async Task GetPresidentsByYear_ReturnsOkWithFilteredData()
//     {
//         int year = 1865; 
//         var response = await _client.GetAsync($"/api/v1/President/year/{year}");

//         response.EnsureSuccessStatusCode();

//         var result = await response.Content.ReadAsStringAsync();
        
//         Assert.NotNull(result);
//         Assert.Contains("Rafael", result); 
//     }

//     [Fact]
//     public async Task SearchPresidents_ReturnsOkWithFilteredData()
//     {
//         string searchKeyword = "Rafael"; 
//         var response = await _client.GetAsync($"/api/v1/President/search/{searchKeyword}");

//         response.EnsureSuccessStatusCode();

//         var result = await response.Content.ReadAsStringAsync();
        
//         Assert.NotNull(result);
//         Assert.Contains("Rafael", result); 
//     }

//     // [Fact]
//     // public async Task GetPagedPresidents_ReturnsOkWithPagedData()
//     // {
//     //     var pagination = new PaginationModel
//     //     {
//     //         Page = 1,
//     //         PageSize = 10
//     //     }; 
//     //     var response = await _client.GetAsync($"/api/v1/President/pagedList?page={pagination.Page}&pageSize={pagination.PageSize}");

//     //     response.EnsureSuccessStatusCode();

//     //     var result = await response.Content.ReadAsStringAsync();

//     //     Assert.NotNull(result);
//     //     Assert.Contains("TotalRecords", result);  
//     // }
// }
