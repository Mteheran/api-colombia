// using System.Net.Http;
// using System.Threading.Tasks;
// using Xunit;
// using Microsoft.AspNetCore.Mvc.Testing;
// using System.Collections.Generic;

// public class IndigenousReservationApiIntegrationTests : IClassFixture<CustomWebApplicationFactory>
// {
//     private readonly HttpClient _client;

//     public IndigenousReservationApiIntegrationTests(CustomWebApplicationFactory factory)
//     {
//         _client = factory.CreateClient();  
//     }

//     [Fact]
//     public async Task GetIndigenousReservations_ReturnsOkWithExpectedData()
//     {
//         var response = await _client.GetAsync("/api/v1/IndigenousReservation");

//         response.EnsureSuccessStatusCode();

//         var result = await response.Content.ReadAsStringAsync();

//         Assert.NotNull(result);    
//         Assert.False(string.IsNullOrEmpty(result));  
//     }

//     [Fact]
//     public async Task GetIndigenousReservationById_ReturnsOkWithReservationData()
//     {
//         int id = 1;  
//         var response = await _client.GetAsync($"/api/v1/IndigenousReservation/{id}");

//         response.EnsureSuccessStatusCode();

//         var result = await response.Content.ReadAsStringAsync();

//         Assert.NotNull(result);
//         Assert.Contains("Id", result);  
//     }

//     [Fact]
//     public async Task GetIndigenousReservationByName_ReturnsOkWithReservationData()
//     {
//         string name = "Sample Reservation";  
//         var response = await _client.GetAsync($"/api/v1/IndigenousReservation/name/{name}");

//         response.EnsureSuccessStatusCode();

//         var result = await response.Content.ReadAsStringAsync();

//         Assert.NotNull(result);
//         Assert.Contains("Name", result);  
//     }

//     [Fact]
//     public async Task SearchIndigenousReservations_ReturnsOkWithFilteredReservations()
//     {
//         string keyword = "Sample";  
//         var response = await _client.GetAsync($"/api/v1/IndigenousReservation/search/{keyword}");

//         response.EnsureSuccessStatusCode();

//         var result = await response.Content.ReadAsStringAsync();

//         Assert.NotNull(result);
//         Assert.Contains("IndigenousReservations", result);  
//     }

//     [Fact]
//     public async Task GetPagedIndigenousReservations_ReturnsOkWithPagedResults()
//     {
//         int page = 1;
//         int pageSize = 10;
//         var response = await _client.GetAsync($"/api/v1/IndigenousReservation/pagedList?page={page}&pageSize={pageSize}");

//         response.EnsureSuccessStatusCode();

//         var result = await response.Content.ReadAsStringAsync();

//         Assert.NotNull(result);
//         Assert.Contains("Data", result);  
//     }

//     [Fact]
//     public async Task GetIndigenousReservationsWithSorting_ReturnsOkWithSortedData()
//     {
//         string sortBy = "Name";  
//         string sortDirection = "asc";  

//         var response = await _client.GetAsync($"/api/v1/IndigenousReservation?sortBy={sortBy}&sortDirection={sortDirection}");

//         response.EnsureSuccessStatusCode();

//         var result = await response.Content.ReadAsStringAsync();

//         Assert.NotNull(result);
//         Assert.Contains("Name", result);  
//     }
// }