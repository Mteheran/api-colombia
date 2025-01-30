// using System.Net.Http;
// using System.Threading.Tasks;
// using Xunit;
// using Microsoft.AspNetCore.Mvc.Testing;
// using api.Models;

// public class CountryApiIntegrationTests : IClassFixture<CustomWebApplicationFactory>
// {
//     private readonly HttpClient _client;

//     public CountryApiIntegrationTests(CustomWebApplicationFactory factory)
//     {
//         _client = factory.CreateClient();  
//     }

//     [Fact]
//     public async Task GetCountry_ReturnsAllCountryData()
//     {
//         var response = await _client.GetAsync("/api/v1/Country");

//         response.EnsureSuccessStatusCode();

//         var result = await response.Content.ReadAsStringAsync();
 
//         Assert.NotNull(result);
//         Assert.NotEmpty(result);  
 
//         var countries = System.Text.Json.JsonSerializer.Deserialize<List<Country>>(result);
//         Assert.NotEmpty(countries);   
//     } 

// }
