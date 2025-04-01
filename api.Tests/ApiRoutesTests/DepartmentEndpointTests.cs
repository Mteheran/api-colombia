using System.Net.Http.Json;
using api.Models;
using api.Utils;

public class DepartmentApiIntegrationTests : IClassFixture<CustomWebApplicationFactory> , IDisposable
{
    private readonly HttpClient _client;

    public DepartmentApiIntegrationTests()
    {
       _client = new CustomWebApplicationFactory().CreateClient(); 
    }

    public void Dispose()
    {
        _client.Dispose();
    }

    [Fact]
    public async Task GetDepartments_ReturnsOkWithExpectedData()
    {
        var response = await _client.GetAsync("/api/v1/Department");
        
        response.EnsureSuccessStatusCode();
        
        var result = await response.Content.ReadAsStringAsync();
        
        Assert.NotNull(result);    
        Assert.False(string.IsNullOrEmpty(result));  
    }

    [Fact]
    public async Task GetDepartmentById_ReturnsOkWithDepartmentData()
    {
        int departmentId = 1;  
        var response = await _client.GetAsync($"/api/v1/Department/{departmentId}");

        response.EnsureSuccessStatusCode();

        var result = await response.Content.ReadFromJsonAsync<Department>();
        
        Assert.NotNull(result);
        Assert.Equal(1, result.Id);  
    }

    [Fact]
    public async Task GetDepartmentById_ReturnsBadRequest()
    {
        int departmentId = 0;  
        var response = await _client.GetAsync($"/api/v1/Department/{departmentId}");
        
        Assert.False(response.IsSuccessStatusCode);
        Assert.Equal(System.Net.HttpStatusCode.BadRequest, response.StatusCode);
    }


    [Fact]
    public async Task GetCitiesByDepartment_ReturnsOkWithCitiesData()
    {
        int departmentId = 1;  
        var response = await _client.GetAsync($"/api/v1/Department/{departmentId}/cities");

        response.EnsureSuccessStatusCode();

        var result = await response.Content.ReadFromJsonAsync<List<City>>();
        
        Assert.NotNull(result);
        Assert.Single(result);
    }

    [Fact]
    public async Task GetNaturalAreasByDepartment_ReturnsOkWithNaturalAreasData()
    {
        int departmentId = 1;  
        var response = await _client.GetAsync($"/api/v1/Department/{departmentId}/naturalareas");

        response.EnsureSuccessStatusCode();

        var result = await response.Content.ReadFromJsonAsync<List<NaturalArea>>();
        
        Assert.NotNull(result);
        Assert.Single(result); 
    }

    [Fact]
    public async Task GetTouristicAttractionsByDepartment_ReturnsOkWithTouristicAttractionsData()
    {
        int departmentId = 1;  
        var response = await _client.GetAsync($"/api/v1/Department/{departmentId}/touristicattractions");

        response.EnsureSuccessStatusCode();

        var result = await response.Content.ReadFromJsonAsync<List<TouristAttraction>>();
        
        Assert.NotNull(result);
        Assert.Equal(3, result.Count);  
    }

    [Fact]
    public async Task GetDepartmentByName_ReturnsOkWithDepartmentData()
    {
        string departmentName = "Antioquia";  
        var response = await _client.GetAsync($"/api/v1/Department/name/{departmentName}");

        response.EnsureSuccessStatusCode();

        var result = await response.Content.ReadFromJsonAsync<List<Department>>();
        
        Assert.NotNull(result);
        Assert.Single(result);
        Assert.Equal(1, result[0].Id);
        Assert.Equal(departmentName, result[0].Name);  
    }

    [Fact]
    public async Task SearchDepartments_ReturnsOkWithFilteredDepartmentsData()
    {
        string keyword = "Anti";  
        var response = await _client.GetAsync($"/api/v1/Department/search/{keyword}");

        response.EnsureSuccessStatusCode();

        var result = await response.Content.ReadFromJsonAsync<List<Department>>();        
        Assert.NotNull(result);
        Assert.Single(result);
        Assert.Contains(keyword, result[0].Name);
    }

    [Fact]
    public async Task GetPagedDepartments_ReturnsOkWithPagedDepartmentsData()
    {
        string sortBy = "Name"; 
        string sortDirection = "asc";  
        int page = 1;
        int pageSize = 10;
        var response = await _client.GetAsync($"/api/v1/Department/pagedList?page={page}&pageSize={pageSize}&sortBy={sortBy}&sortDirection={sortDirection}");

        response.EnsureSuccessStatusCode();

        var result = await response.Content.ReadFromJsonAsync<PaginationResponseModel<Department>>(); 
        
        Assert.NotNull(result);
        Assert.Equal(2, result.Data.Count);  
    }

    [Fact]
    public async Task GetDepartmentsWithSorting_ReturnsOkWithSortedData()
    {
        string sortBy = "Name"; 
        string sortDirection = "asc";  

        var response = await _client.GetAsync($"/api/v1/Department?sortBy={sortBy}&sortDirection={sortDirection}");

        response.EnsureSuccessStatusCode();

       var result = await response.Content.ReadFromJsonAsync<List<Department>>(); 
        
        Assert.NotNull(result);
        Assert.Equal(2, result.Count);  
    }
}
