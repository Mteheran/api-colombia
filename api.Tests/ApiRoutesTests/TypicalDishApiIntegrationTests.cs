// using System.Net.Http;
// using System.Threading.Tasks;
// using Xunit;
// using Microsoft.AspNetCore.Mvc.Testing;

// public class TypicalDishApiIntegrationTests : IClassFixture<CustomWebApplicationFactory>
// {
//     private readonly HttpClient _client;

//     public TypicalDishApiIntegrationTests(CustomWebApplicationFactory factory)
//     {
//         _client = factory.CreateClient();
//     }

//     [Fact]
//     public async Task GetTypicalDishes_ReturnsOkWithExpectedData()
//     {
//         var response = await _client.GetAsync("/api/v1/TypicalDish");

//         response.EnsureSuccessStatusCode();

//         var result = await response.Content.ReadAsStringAsync();

//         Assert.NotNull(result);
//         Assert.False(string.IsNullOrEmpty(result));
//     }

//     [Fact]
//     public async Task GetTypicalDishById_ReturnsOkWithTypicalDishData()
//     {
//         int typicalDishId = 1;  
//         var response = await _client.GetAsync($"/api/v1/TypicalDish/{typicalDishId}");

//         response.EnsureSuccessStatusCode();

//         var result = await response.Content.ReadAsStringAsync();

//         Assert.NotNull(result);
//         Assert.Contains("Id", result);
//     }

//     [Fact]
//     public async Task GetTypicalDishByDepartment_ReturnsOkWithTypicalDishData()
//     {
//         int departmentId = 1;  
//         var response = await _client.GetAsync($"/api/v1/TypicalDish/{departmentId}/department");

//         response.EnsureSuccessStatusCode();

//         var result = await response.Content.ReadAsStringAsync();

//         Assert.NotNull(result);
//         Assert.Contains("DepartmentId", result);
//     }

//     [Fact]
//     public async Task GetTypicalDishByName_ReturnsOkWithTypicalDishData()
//     {
//         string dishName = "TestDish";  
//         var response = await _client.GetAsync($"/api/v1/TypicalDish/name/{dishName}");

//         response.EnsureSuccessStatusCode();

//         var result = await response.Content.ReadAsStringAsync();

//         Assert.NotNull(result);
//         Assert.Contains("TestDish", result);
//     }

//     [Fact]
//     public async Task SearchTypicalDishes_ReturnsOkWithFilteredData()
//     {
//         string searchKeyword = "Test";  
//         var response = await _client.GetAsync($"/api/v1/TypicalDish/search/{searchKeyword}");

//         response.EnsureSuccessStatusCode();

//         var result = await response.Content.ReadAsStringAsync();

//         Assert.NotNull(result);
//         Assert.Contains("Test", result);
//     }

//     [Fact]
//     public async Task GetPagedTypicalDishes_ReturnsOkWithPagedData()
//     {
//         int page = 1;
//         int pageSize = 10;

//         var response = await _client.GetAsync($"/api/v1/TypicalDish/pagedList?page={page}&pageSize={pageSize}");

//         response.EnsureSuccessStatusCode();

//         var result = await response.Content.ReadAsStringAsync();

//         Assert.NotNull(result);
//         Assert.Contains("TotalRecords", result);
//     }
// }
