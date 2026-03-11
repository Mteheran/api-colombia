using System.Net.Http.Json;
using api.Models;
using api.Utils;

namespace api.Tests.ApiRoutesTests;

public class NativeCommunityApiIntegrationTests : IClassFixture<CustomWebApplicationFactory>
{
    private readonly HttpClient _client;

    public NativeCommunityApiIntegrationTests(CustomWebApplicationFactory factory)
    {
        factory.ResetDatabase();
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task GetNativeCommunities_ReturnsOkWithExpectedData()
    {
        var response = await _client.GetAsync("/api/v1/NativeCommunity");
        
        response.EnsureSuccessStatusCode();
        
        var result = await response.Content.ReadFromJsonAsync<List<NativeCommunity>>();
        
        Assert.NotNull(result);
        Assert.Equal(3, result.Count);
    }

    [Fact]
    public async Task GetNativeCommunityById_ReturnsOkWithNativeCommunityData()
    {
        int communityId = 1;  
        var response = await _client.GetAsync($"/api/v1/NativeCommunity/{communityId}");

        response.EnsureSuccessStatusCode();

        var result = await response.Content.ReadFromJsonAsync<NativeCommunity>();
        
        Assert.NotNull(result);
        Assert.Equal(communityId, result.Id);
        Assert.NotNull(result.Name);
        Assert.NotNull(result.Description); 
    }

    [Fact]
    public async Task GetNativeCommunityById_ReturnsBadRequest()
    {
        int communityId = 0;  
        var response = await _client.GetAsync($"/api/v1/NativeCommunity/{communityId}");

        Assert.False(response.IsSuccessStatusCode);
        Assert.Equal(System.Net.HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task GetNativeCommunityByName_ReturnsOkWithNativeCommunityData()
    {
        string communityName = "Sample Community";  
        var response = await _client.GetAsync($"/api/v1/NativeCommunity/name/{communityName}");

        response.EnsureSuccessStatusCode();

        var result = await response.Content.ReadFromJsonAsync<List<NativeCommunity>>();
        
        Assert.NotNull(result);
        Assert.Equal(communityName, result[0].Name);
        Assert.NotNull(result[0].Description);  
    }

    [Fact]
    public async Task SearchNativeCommunities_ReturnsOkWithFilteredNativeCommunitiesData()
    {
        string keyword = "Sample";  
        var response = await _client.GetAsync($"/api/v1/NativeCommunity/search/{keyword}");

        response.EnsureSuccessStatusCode();

        var result = await response.Content.ReadFromJsonAsync<List<NativeCommunity>>();
        
        Assert.NotNull(result);
        Assert.All(result, community => Assert.Contains(keyword, community.Name));
        Assert.Single(result);
    }

    [Fact]
    public async Task GetPagedNativeCommunities_ReturnsOkWithPagedNativeCommunitiesData()
    {
        string sortBy = "Name"; 
        string sortDirection = "asc";  
        int page = 1;
        int pageSize = 1;
        var response = await _client.GetAsync($"/api/v1/NativeCommunity/pagedList?page={page}&pageSize={pageSize}&sortBy={sortBy}&sortDirection={sortDirection}");

        response.EnsureSuccessStatusCode();

        var result = await response.Content.ReadFromJsonAsync<PaginationResponseModel<NativeCommunity>>();
        
        Assert.NotNull(result);
        Assert.Equal(1, result.Page);
        Assert.Equal(1, result.PageSize);
        Assert.Equal(3, result.TotalRecords);
    }

    [Fact]
    public async Task GetNativeCommunitiesWithSorting_ReturnsOkWithSortedData()
    {
        string sortBy = "Name"; 
        string sortDirection = "asc";  

        var response = await _client.GetAsync($"/api/v1/NativeCommunity?sortBy={sortBy}&sortDirection={sortDirection}");

        response.EnsureSuccessStatusCode();

        var result = await response.Content.ReadFromJsonAsync<List<NativeCommunity>>();
        
        Assert.NotNull(result);
        Assert.Equal(3, result.Count);
        Assert.Equal("Another Community", result[0].Name); 
    }
}