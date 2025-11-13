using System.Net.Http.Json;
using api.Models;
using api.Utils;

namespace api.Tests.ApiRoutesTests;

public class TypicalDishApiIntegrationTests(CustomWebApplicationFactory factory)
    : IClassFixture<CustomWebApplicationFactory>
{
    private readonly HttpClient _client = factory.CreateClient();

    [Fact]
    public async Task GetTypicalDishes_ReturnsOkWithExpectedData()
    {
        var response = await _client.GetAsync("/api/v1/TypicalDish");

        response.EnsureSuccessStatusCode();

        var result = await response.Content.ReadFromJsonAsync<List<TypicalDish>>(); 

        Assert.NotNull(result);
        Assert.NotEmpty(result);
        Assert.Equal(3, result.Count);
    }

    [Fact]
    public async Task GetTypicalDishById_ReturnsOkWithTypicalDishData()
    {
        int typicalDishId = 1;  
        var response = await _client.GetAsync($"/api/v1/TypicalDish/{typicalDishId}");

        response.EnsureSuccessStatusCode();

        var result = await response.Content.ReadFromJsonAsync<TypicalDish>(); 

        Assert.NotNull(result);
        Assert.Equal(typicalDishId, result.Id);
        Assert.Equal("Bandeja Paisa", result.Name);
    }

    [Fact]
    public async Task GetTypicalDishById_ReturnsBadRequest()
    {
        int typicalDishId = 0;  
        var response = await _client.GetAsync($"/api/v1/TypicalDish/{typicalDishId}");

        Assert.False(response.IsSuccessStatusCode);
        Assert.Equal(System.Net.HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task GetTypicalDishByDepartment_ReturnsOkWithTypicalDishData()
    {
        int departmentId = 1;  
        var response = await _client.GetAsync($"/api/v1/TypicalDish/{departmentId}/department");

        response.EnsureSuccessStatusCode();

        var result = await response.Content.ReadFromJsonAsync<List<TypicalDish>>(); 

        Assert.NotNull(result);
        Assert.NotEmpty(result);
        Assert.Equal(2, result.Count);
        Assert.Equal(departmentId, result[0].DepartmentId);
    }

    [Fact]
    public async Task GetTypicalDishByName_ReturnsOkWithTypicalDishData()
    {
        string dishName = "Sancocho";  
        var response = await _client.GetAsync($"/api/v1/TypicalDish/name/{dishName}");

        response.EnsureSuccessStatusCode();

        var result = await response.Content.ReadFromJsonAsync<List<TypicalDish>>(); 

        Assert.NotNull(result);
        Assert.NotEmpty(result);
        Assert.Equal(dishName, result[0].Name, StringComparer.OrdinalIgnoreCase);
        Assert.Single(result);
    }

    [Fact]
    public async Task SearchTypicalDishes_ReturnsOkWithFilteredData()
    {
        string searchKeyword = "arep";  
        var response = await _client.GetAsync($"/api/v1/TypicalDish/search/{searchKeyword}");

        response.EnsureSuccessStatusCode();

        var result = await response.Content.ReadFromJsonAsync<List<TypicalDish>>(); 

        Assert.NotNull(result);
        Assert.NotEmpty(result);
        Assert.All(result, dish => Assert.Contains(searchKeyword, dish.Name, StringComparison.OrdinalIgnoreCase));
        Assert.Single(result);
    }

    [Fact]
    public async Task GetPagedTypicalDishes_ReturnsOkWithPagedData()
    {
        int page = 1;
        int pageSize = 2;

        var response = await _client.GetAsync($"/api/v1/TypicalDish/pagedList?page={page}&pageSize={pageSize}");

        response.EnsureSuccessStatusCode();

        var result = await response.Content.ReadFromJsonAsync<PaginationResponseModel<TypicalDish>>(); 

        Assert.NotNull(result);
        Assert.Equal(page, result.Page);
        Assert.Equal(pageSize, result.PageSize);
        Assert.Equal(3, result.TotalRecords);
    }
}