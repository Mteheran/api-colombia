// using System.Net.Http;
// using System.Threading.Tasks;
// using Xunit;
// using Microsoft.AspNetCore.Mvc.Testing;

// public class NativeCommunityApiIntegrationTests : IClassFixture<CustomWebApplicationFactory>
// {
//     private readonly HttpClient _client;

//     public NativeCommunityApiIntegrationTests(CustomWebApplicationFactory factory)
//     {
//         _client = factory.CreateClient();
//     }

//     [Fact]
//     public async Task GetNativeCommunities_ReturnsOkWithExpectedData()
//     {
//         var response = await _client.GetAsync("/api/v1/NativeCommunity");
        
//         response.EnsureSuccessStatusCode();
        
//         var result = await response.Content.ReadAsStringAsync();
        
//         Assert.NotNull(result);    
//         Assert.False(string.IsNullOrEmpty(result));  
//     }

//     [Fact]
//     public async Task GetNativeCommunityById_ReturnsOkWithNativeCommunityData()
//     {
//         int communityId = 1;  
//         var response = await _client.GetAsync($"/api/v1/NativeCommunity/{communityId}");

//         response.EnsureSuccessStatusCode();

//         var result = await response.Content.ReadAsStringAsync();
        
//         Assert.NotNull(result);
//         Assert.Contains("Id", result);  
//     }

//     [Fact]
//     public async Task GetNativeCommunityByName_ReturnsOkWithNativeCommunityData()
//     {
//         string communityName = "Sample Community";  
//         var response = await _client.GetAsync($"/api/v1/NativeCommunity/name/{communityName}");

//         response.EnsureSuccessStatusCode();

//         var result = await response.Content.ReadAsStringAsync();
        
//         Assert.NotNull(result);
//         Assert.Contains("Name", result);  
//     }

//     [Fact]
//     public async Task SearchNativeCommunities_ReturnsOkWithFilteredNativeCommunitiesData()
//     {
//         string keyword = "Sample";  
//         var response = await _client.GetAsync($"/api/v1/NativeCommunity/search/{keyword}");

//         response.EnsureSuccessStatusCode();

//         var result = await response.Content.ReadAsStringAsync();
        
//         Assert.NotNull(result);
//         Assert.Contains("NativeCommunities", result);  
//     }

//     [Fact]
//     public async Task GetPagedNativeCommunities_ReturnsOkWithPagedNativeCommunitiesData()
//     {
//         string sortBy = "Name"; 
//         string sortDirection = "asc";  
//         int page = 1;
//         int pageSize = 10;
//         var response = await _client.GetAsync($"/api/v1/NativeCommunity/pagedList?page={page}&pageSize={pageSize}&sortBy={sortBy}&sortDirection={sortDirection}");

//         response.EnsureSuccessStatusCode();

//         var result = await response.Content.ReadAsStringAsync();
        
//         Assert.NotNull(result);
//         Assert.Contains("Data", result);  
//     }

//     [Fact]
//     public async Task GetNativeCommunitiesWithSorting_ReturnsOkWithSortedData()
//     {
//         string sortBy = "Name"; 
//         string sortDirection = "asc";  

//         var response = await _client.GetAsync($"/api/v1/NativeCommunity?sortBy={sortBy}&sortDirection={sortDirection}");

//         response.EnsureSuccessStatusCode();

//         var result = await response.Content.ReadAsStringAsync();
        
//         Assert.NotNull(result);
//         Assert.Contains("Name", result);  
//     }
// }
