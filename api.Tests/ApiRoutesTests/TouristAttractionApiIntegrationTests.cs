using api.Models;
using api.Utils;
using System.Net.Http.Json;

public class TouristAttractionApiIntegrationTests : IClassFixture<CustomWebApplicationFactory>
{
    private readonly HttpClient _client;

    public TouristAttractionApiIntegrationTests(CustomWebApplicationFactory factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task GetTouristAttractions_ReturnsOkWithExpectedData()
    {
        var response = await _client.GetAsync("/api/v1/TouristicAttraction");

        response.EnsureSuccessStatusCode();

       var result = await response.Content.ReadFromJsonAsync<List<TouristAttraction>>();

        Assert.NotNull(result);
    }

    [Fact]
    public async Task GetTouristAttractionById_ReturnsOkWithTouristAttractionData()
    {
        int attractionId = 1;  
        var response = await _client.GetAsync($"/api/v1/TouristicAttraction/{attractionId}");

        response.EnsureSuccessStatusCode();

       var result = await response.Content.ReadFromJsonAsync<TouristAttraction>(); 

        Assert.NotNull(result);
        Assert.Equal(attractionId, result.Id);
        Assert.NotNull(result.Name);
    }

    [Fact]
    public async Task GetTouristAttractionByName_ReturnsOkWithTouristAttractionData()
    {
        string attractionName = "Parque Explora";  
        var response = await _client.GetAsync($"/api/v1/TouristicAttraction/name/{attractionName}");

        response.EnsureSuccessStatusCode();

       var result = await response.Content.ReadFromJsonAsync<List<TouristAttraction>>(); 

        Assert.NotNull(result);
        Assert.Single(result);
        Assert.Contains(result, r => r.Name == attractionName);
    }

    [Fact]
    public async Task SearchTouristAttractions_ReturnsOkWithFilteredData()
    {
        string searchKeyword = "parqu";  
        var response = await _client.GetAsync($"/api/v1/TouristicAttraction/search/{searchKeyword}");

        response.EnsureSuccessStatusCode();

       var result = await response.Content.ReadFromJsonAsync<List<TouristAttraction>>(); 

        Assert.NotNull(result);
        Assert.NotEmpty(result);
        Assert.All(result, attraction => Assert.Contains(searchKeyword, attraction.Name, StringComparison.OrdinalIgnoreCase));
        Assert.Equal(2, result.Count);
    }

    [Fact]
    public async Task GetPagedTouristAttractions_ReturnsOkWithPagedData()
    {
        int page = 1;
        int pageSize = 10;

        var response = await _client.GetAsync($"/api/v1/TouristicAttraction/pagedList?page={page}&pageSize={pageSize}");

        response.EnsureSuccessStatusCode();

       var result = await response.Content.ReadFromJsonAsync<PaginationResponseModel<TouristAttraction>>(); 

        Assert.NotNull(result);
        Assert.Equal(page, result.Page);
        Assert.Equal(pageSize, result.PageSize);
        Assert.Equal(3, result.TotalRecords);
    }
}
