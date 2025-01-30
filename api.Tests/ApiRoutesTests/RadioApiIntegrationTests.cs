// using System.Net.Http;
// using System.Threading.Tasks;
// using Xunit;
// using Microsoft.AspNetCore.Mvc.Testing;

// public class RadioApiIntegrationTests : IClassFixture<CustomWebApplicationFactory>
// {
//     private readonly HttpClient _client;

//     public RadioApiIntegrationTests(CustomWebApplicationFactory factory)
//     {
//         _client = factory.CreateClient();
//     }

//     [Fact]
//     public async Task GetRadios_ReturnsOkWithExpectedData()
//     {
//         var response = await _client.GetAsync("/api/v1/Radio");

//         response.EnsureSuccessStatusCode();

//         var result = await response.Content.ReadAsStringAsync();

//         Assert.NotNull(result);
//         Assert.False(string.IsNullOrEmpty(result));
//     }

//     [Fact]
//     public async Task GetRadioById_ReturnsOkWithRadioData()
//     {
//         int radioId = 1;  
//         var response = await _client.GetAsync($"/api/v1/Radio/{radioId}");

//         response.EnsureSuccessStatusCode();

//         var result = await response.Content.ReadAsStringAsync();

//         Assert.NotNull(result);
//         Assert.Contains("Id", result);
//     }

//     [Fact]
//     public async Task GetRadioByName_ReturnsOkWithRadioData()
//     {
//         string radioName = "TestRadio";  
//         var response = await _client.GetAsync($"/api/v1/Radio/name/{radioName}");

//         response.EnsureSuccessStatusCode();

//         var result = await response.Content.ReadAsStringAsync();

//         Assert.NotNull(result);
//         Assert.Contains("TestRadio", result);
//     }

//     [Fact]
//     public async Task SearchRadios_ReturnsOkWithFilteredData()
//     {
//         string searchKeyword = "Test";  
//         var response = await _client.GetAsync($"/api/v1/Radio/search/{searchKeyword}");

//         response.EnsureSuccessStatusCode();

//         var result = await response.Content.ReadAsStringAsync();

//         Assert.NotNull(result);
//         Assert.Contains("Test", result);
//     }

//     [Fact]
//     public async Task GetPagedRadios_ReturnsOkWithPagedData()
//     {
//         int page = 1;
//         int pageSize = 10;

//         var response = await _client.GetAsync($"/api/v1/Radio/pagedList?page={page}&pageSize={pageSize}");

//         response.EnsureSuccessStatusCode();

//         var result = await response.Content.ReadAsStringAsync();

//         Assert.NotNull(result);
//         Assert.Contains("TotalRecords", result);
//     }
// }
