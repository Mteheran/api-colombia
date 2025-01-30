// using System.Net.Http;
// using System.Threading.Tasks;
// using Xunit;
// using Microsoft.AspNetCore.Mvc.Testing;

// public class DepartmentApiIntegrationTests : IClassFixture<CustomWebApplicationFactory>
// {
//     private readonly HttpClient _client;

//     public DepartmentApiIntegrationTests(CustomWebApplicationFactory factory)
//     {
//         _client = factory.CreateClient();  
//     }

//     [Fact]
//     public async Task GetDepartments_ReturnsOkWithExpectedData()
//     {
//         var response = await _client.GetAsync("/api/v1/Department");
        
//         response.EnsureSuccessStatusCode();
        
//         var result = await response.Content.ReadAsStringAsync();
        
//         Assert.NotNull(result);    
//         Assert.False(string.IsNullOrEmpty(result));  
//     }

//     [Fact]
//     public async Task GetDepartmentById_ReturnsOkWithDepartmentData()
//     {
//         int departmentId = 1;  
//         var response = await _client.GetAsync($"/api/v1/Department/{departmentId}");

//         response.EnsureSuccessStatusCode();

//         var result = await response.Content.ReadAsStringAsync();
        
//         Assert.NotNull(result);
//         Assert.Contains("Id", result);  
//     }

//     [Fact]
//     public async Task GetCitiesByDepartment_ReturnsOkWithCitiesData()
//     {
//         int departmentId = 1;  
//         var response = await _client.GetAsync($"/api/v1/Department/{departmentId}/cities");

//         response.EnsureSuccessStatusCode();

//         var result = await response.Content.ReadAsStringAsync();
        
//         Assert.NotNull(result);
//         Assert.Contains("Cities", result);  
//     }

//     [Fact]
//     public async Task GetNaturalAreasByDepartment_ReturnsOkWithNaturalAreasData()
//     {
//         int departmentId = 1;  
//         var response = await _client.GetAsync($"/api/v1/Department/{departmentId}/naturalareas");

//         response.EnsureSuccessStatusCode();

//         var result = await response.Content.ReadAsStringAsync();
        
//         Assert.NotNull(result);
//         Assert.Contains("NaturalAreas", result);  
//     }

//     [Fact]
//     public async Task GetTouristicAttractionsByDepartment_ReturnsOkWithTouristicAttractionsData()
//     {
//         int departmentId = 1;  
//         var response = await _client.GetAsync($"/api/v1/Department/{departmentId}/touristicattractions");

//         response.EnsureSuccessStatusCode();

//         var result = await response.Content.ReadAsStringAsync();
        
//         Assert.NotNull(result);
//         Assert.Contains("TouristicAttractions", result);  
//     }

//     [Fact]
//     public async Task GetDepartmentByName_ReturnsOkWithDepartmentData()
//     {
//         string departmentName = "Sample Department";  
//         var response = await _client.GetAsync($"/api/v1/Department/name/{departmentName}");

//         response.EnsureSuccessStatusCode();

//         var result = await response.Content.ReadAsStringAsync();
        
//         Assert.NotNull(result);
//         Assert.Contains("Name", result);  
//     }

//     [Fact]
//     public async Task SearchDepartments_ReturnsOkWithFilteredDepartmentsData()
//     {
//         string keyword = "Sample";  
//         var response = await _client.GetAsync($"/api/v1/Department/search/{keyword}");

//         response.EnsureSuccessStatusCode();

//         var result = await response.Content.ReadAsStringAsync();
        
//         Assert.NotNull(result);
//         Assert.Contains("Departments", result);  
//     }

//     [Fact]
//     public async Task GetPagedDepartments_ReturnsOkWithPagedDepartmentsData()
//     {
//         string sortBy = "Name"; 
//         string sortDirection = "asc";  
//         int page = 1;
//         int pageSize = 10;
//         var response = await _client.GetAsync($"/api/v1/Department/pagedList?page={page}&pageSize={pageSize}&sortBy={sortBy}&sortDirection={sortDirection}");

//         response.EnsureSuccessStatusCode();

//         var result = await response.Content.ReadAsStringAsync();
        
//         Assert.NotNull(result);
//         Assert.Contains("Data", result);  
//     }

//     [Fact]
//     public async Task GetDepartmentsWithSorting_ReturnsOkWithSortedData()
//     {
//         string sortBy = "Name"; 
//         string sortDirection = "asc";  

//         var response = await _client.GetAsync($"/api/v1/Department?sortBy={sortBy}&sortDirection={sortDirection}");

//         response.EnsureSuccessStatusCode();

//         var result = await response.Content.ReadAsStringAsync();
        
//         Assert.NotNull(result);
//         Assert.Contains("Name", result);  
//     }
// }
