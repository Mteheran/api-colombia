// using System.Net.Http;
// using System.Threading.Tasks;
// using Xunit;
// using Microsoft.AspNetCore.Mvc.Testing;
// using System.Collections.Generic;

// public class InvasiveSpecieApiIntegrationTests : IClassFixture<CustomWebApplicationFactory>
// {
//     private readonly HttpClient _client;

//     public InvasiveSpecieApiIntegrationTests(CustomWebApplicationFactory factory)
//     {
//         _client = factory.CreateClient();  
//     }

//     [Fact]
//     public async Task GetInvasiveSpecies_ReturnsOkWithExpectedData()
//     {
//         var response = await _client.GetAsync("/api/v1/InvasiveSpecie");

//         response.EnsureSuccessStatusCode();

//         var result = await response.Content.ReadAsStringAsync();

//         Assert.NotNull(result);    
//         Assert.False(string.IsNullOrEmpty(result));  
//     }

//     [Fact]
//     public async Task GetInvasiveSpecieById_ReturnsOkWithSpecieData()
//     {
//         int id = 1;  
//         var response = await _client.GetAsync($"/api/v1/InvasiveSpecie/{id}");

//         response.EnsureSuccessStatusCode();

//         var result = await response.Content.ReadAsStringAsync();

//         Assert.NotNull(result);
//         Assert.Contains("Id", result);  
//     }

//     [Fact]
//     public async Task GetInvasiveSpecieByName_ReturnsOkWithSpecieData()
//     {
//         string name = "Sample Specie";  
//         var response = await _client.GetAsync($"/api/v1/InvasiveSpecie/name/{name}");

//         response.EnsureSuccessStatusCode();

//         var result = await response.Content.ReadAsStringAsync();

//         Assert.NotNull(result);
//         Assert.Contains("Name", result);  
//     }

//     [Fact]
//     public async Task SearchInvasiveSpecies_ReturnsOkWithFilteredSpecies()
//     {
//         string keyword = "Sample";  
//         var response = await _client.GetAsync($"/api/v1/InvasiveSpecie/search/{keyword}");

//         response.EnsureSuccessStatusCode();

//         var result = await response.Content.ReadAsStringAsync();

//         Assert.NotNull(result);
//         Assert.Contains("InvasiveSpecies", result);  
//     }

//     [Fact]
//     public async Task GetPagedInvasiveSpecies_ReturnsOkWithPagedResults()
//     {
//         int page = 1;
//         int pageSize = 10;
//         var response = await _client.GetAsync($"/api/v1/InvasiveSpecie/pagedList?page={page}&pageSize={pageSize}");

//         response.EnsureSuccessStatusCode();

//         var result = await response.Content.ReadAsStringAsync();

//         Assert.NotNull(result);
//         Assert.Contains("Data", result);  
//     }

//     [Fact]
//     public async Task GetInvasiveSpeciesWithSorting_ReturnsOkWithSortedData()
//     {
//         string sortBy = "Name";  
//         string sortDirection = "asc";  

//         var response = await _client.GetAsync($"/api/v1/InvasiveSpecie?sortBy={sortBy}&sortDirection={sortDirection}");

//         response.EnsureSuccessStatusCode();

//         var result = await response.Content.ReadAsStringAsync();

//         Assert.NotNull(result);
//         Assert.Contains("Name", result);  
//     }
// }
