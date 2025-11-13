using System.Net;
using System.Net.Http.Json;
using api.Models;
using api.Utils;

namespace api.Tests.ApiRoutesTests;

public class IndigenousReservationApiIntegrationTests(CustomWebApplicationFactory factory)
    : IClassFixture<CustomWebApplicationFactory>
{
    private readonly HttpClient _client = factory.CreateClient();

    [Fact]
    public async Task GetIndigenousReservations_ReturnsOkWithExpectedData()
    {
        var response = await _client.GetAsync("/api/v1/IndigenousReservation");

        response.EnsureSuccessStatusCode();

        var result = await response.Content.ReadFromJsonAsync<List<IndigenousReservation>>();

        Assert.NotNull(result);
        Assert.Equal(3, result.Count);
    }

    [Fact]
    public async Task GetIndigenousReservationById_ReturnsOkWithReservationData()
    {
        int id = 1;  
        var response = await _client.GetAsync($"/api/v1/IndigenousReservation/{id}");

        response.EnsureSuccessStatusCode();

        var result = await response.Content.ReadFromJsonAsync<IndigenousReservation>();

        Assert.NotNull(result);
        Assert.Equal(id, result.Id);
    }

    [Fact]
    public async Task GetIndigenousReservationById_ReturnsBadRequest()
    {
        int id = 0;  
        var response = await _client.GetAsync($"/api/v1/IndigenousReservation/{id}");

        Assert.False(response.IsSuccessStatusCode);
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task GetIndigenousReservationByName_ReturnsOkWithReservationData()
    {
        string name = "Wayuu";  
        var response = await _client.GetAsync($"/api/v1/IndigenousReservation/name/{name}");

        response.EnsureSuccessStatusCode();

        var result = await response.Content.ReadFromJsonAsync<List<IndigenousReservation>>();

        Assert.NotNull(result);
        Assert.Equal(name, result[0].Name);
    }

    [Fact]
    public async Task SearchIndigenousReservations_ReturnsOkWithFilteredReservations()
    {
        string keyword = "Embe";  
        var response = await _client.GetAsync($"/api/v1/IndigenousReservation/search/{keyword}");

        response.EnsureSuccessStatusCode();

        var result = await response.Content.ReadFromJsonAsync<List<IndigenousReservation>>();

        Assert.NotNull(result);
        Assert.Single(result);
        Assert.Contains(result, r => r.Name.Contains(keyword, StringComparison.OrdinalIgnoreCase));
    }

    [Fact]
    public async Task GetPagedIndigenousReservations_ReturnsOkWithPagedResults()
    {
        int page = 1;
        int pageSize = 1;
        var response = await _client.GetAsync($"/api/v1/IndigenousReservation/pagedList?page={page}&pageSize={pageSize}");

        response.EnsureSuccessStatusCode();

        var result = await response.Content.ReadFromJsonAsync<PaginationResponseModel<IndigenousReservation>>();

        Assert.NotNull(result);
        Assert.Equal(pageSize, result.PageSize);
        Assert.Equal(page, result.Page);
        Assert.Equal(3, result.TotalRecords);
        Assert.All(result.Data, r => Assert.True(r.Id > 0));
    }

    [Fact]
    public async Task GetIndigenousReservationsWithSorting_ReturnsOkWithSortedData()
    {
        string sortBy = "Name";  
        string sortDirection = "desc";  

        var response = await _client.GetAsync($"/api/v1/IndigenousReservation?sortBy={sortBy}&sortDirection={sortDirection}");

        response.EnsureSuccessStatusCode();

        var result = await response.Content.ReadFromJsonAsync<List<IndigenousReservation>>();
        
        Assert.NotNull(result);
        Assert.Equal(3, result.Count);
        Assert.Equal("Zenú", result[0].Name);
        Assert.Equal("Wayuu", result[1].Name);
        Assert.Equal("Emberá", result[2].Name);
    }
}