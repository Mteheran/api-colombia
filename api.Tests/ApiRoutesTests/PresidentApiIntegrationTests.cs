using System.Net.Http.Json;
using api.Models;
using api.Utils;

public class PresidentApiIntegrationTests : IClassFixture<CustomWebApplicationFactory>
{
    private readonly HttpClient _client;

    public PresidentApiIntegrationTests(CustomWebApplicationFactory factory)
    {
        _client = factory.CreateClient();  
    }

    [Fact]
    public async Task GetPresidents_ReturnsOkWithExpectedData()
    {   
        var response = await _client.GetAsync("/api/v1/President");

        response.EnsureSuccessStatusCode();

         var result = await response.Content.ReadFromJsonAsync<List<President>>();

        Assert.NotNull(result);
        Assert.Equal(3, result.Count);
        Assert.Contains(result, p => p.Name == "Rafael");
    }

    [Fact]
    public async Task GetPresidentById_ReturnsOkWithPresidentData()
    {
        int presidentId = 1;  
        var response = await _client.GetAsync($"/api/v1/President/{presidentId}");

        response.EnsureSuccessStatusCode();

        var result = await response.Content.ReadFromJsonAsync<President>();
        
        Assert.NotNull(result);
        Assert.Equal(presidentId, result.Id);
    }

    [Fact]
    public async Task GetPresidentByName_ReturnsOkWithPresidentData()
    {
        string presidentName = "Rafael";  
        var response = await _client.GetAsync($"/api/v1/President/name/{presidentName}");

        response.EnsureSuccessStatusCode();

        var result = await response.Content.ReadFromJsonAsync<List<President>>();
        
        Assert.NotNull(result);
        Assert.Equal(presidentName, result[0].Name);
        Assert.NotNull(result[0].Description);
    }

    [Fact]
    public async Task GetPresidentsByYear_ReturnsOkWithFilteredData()
    {
        int year = 1865; 
        var response = await _client.GetAsync($"/api/v1/President/year/{year}");

        response.EnsureSuccessStatusCode();

        var result = await response.Content.ReadFromJsonAsync<List<President>>();
        
        Assert.NotNull(result);
        Assert.All(result, p => Assert.Equal(year, p.StartPeriodDate.Year));
    }

    [Fact]
    public async Task SearchPresidents_ReturnsOkWithFilteredData()
    {
        string searchKeyword = "president"; 
        var response = await _client.GetAsync($"/api/v1/President/search/{searchKeyword}");

        response.EnsureSuccessStatusCode();

        var result = await response.Content.ReadFromJsonAsync<List<President>>();
        
        Assert.NotNull(result);
        Assert.Equal(2, result.Count);
        Assert.All(result, p => Assert.Contains(searchKeyword, p.Name, StringComparison.OrdinalIgnoreCase));
    }

    [Fact]
    public async Task GetPagedPresidents_ReturnsOkWithPagedData()
    {
        var pagination = new PaginationModel
        {
            Page = 1,
            PageSize = 2
        }; 
        var response = await _client.GetAsync($"/api/v1/President/pagedList?page={pagination.Page}&pageSize={pagination.PageSize}");

        response.EnsureSuccessStatusCode();

        var result = await response.Content.ReadFromJsonAsync<PaginationResponseModel<President>>();

        Assert.NotNull(result);
        Assert.Equal(pagination.Page, result.Page);
        Assert.Equal(pagination.PageSize, result.PageSize);
        Assert.Equal(3, result.TotalRecords);
    }
}
