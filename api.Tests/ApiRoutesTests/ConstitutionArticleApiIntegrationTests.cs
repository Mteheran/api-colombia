// using System.Net.Http;
// using System.Threading.Tasks;
// using Xunit;
// using Microsoft.AspNetCore.Mvc.Testing;

// public class ConstitutionArticleApiIntegrationTests : IClassFixture<CustomWebApplicationFactory>
// {
//     private readonly HttpClient _client;

//     public ConstitutionArticleApiIntegrationTests(CustomWebApplicationFactory factory)
//     {
//         _client = factory.CreateClient();  
//     }

//     [Fact]
//     public async Task GetConstitutionArticles_ReturnsOkWithExpectedData()
//     {   
//         var response = await _client.GetAsync("/api/v1/ConstitutionArticle");
        
//         response.EnsureSuccessStatusCode();
        
//         var result = await response.Content.ReadAsStringAsync();
        
//         Assert.NotNull(result);    
//         Assert.False(string.IsNullOrEmpty(result));  
//     }

//     [Fact]
//     public async Task GetConstitutionArticleById_ReturnsOkWithConstitutionArticleData()
//     {
//         int constitutionArticleId = 1;  
//         var response = await _client.GetAsync($"/api/v1/ConstitutionArticle/{constitutionArticleId}");

//         response.EnsureSuccessStatusCode();

//         var result = await response.Content.ReadAsStringAsync();
        
//         Assert.NotNull(result);
//         Assert.Contains("Id", result);  
//     }

//     [Fact]
//     public async Task GetConstitutionArticlesWithSorting_ReturnsOkWithSortedData()
//     {
//         string sortBy = "ChapterNumber"; 
//         string sortDirection = "asc";  

//         var response = await _client.GetAsync($"/api/v1/ConstitutionArticle?sortBy={sortBy}&sortDirection={sortDirection}");

//         response.EnsureSuccessStatusCode();

//         var result = await response.Content.ReadAsStringAsync();
        
//         Assert.NotNull(result);
//         Assert.Contains("ChapterNumber", result);  
//     }

//     [Fact]
//     public async Task SearchConstitutionArticles_ReturnsOkWithFilteredData()
//     {
//         string keyword = "Rights";
//         var response = await _client.GetAsync($"/api/v1/ConstitutionArticle/search/{keyword}");

//         response.EnsureSuccessStatusCode();

//         var result = await response.Content.ReadAsStringAsync();
        
//         Assert.NotNull(result);
//         Assert.Contains("Rights", result);  
//     }

//     [Fact]
//     public async Task GetPagedConstitutionArticles_ReturnsOkWithPagedData()
//     {
//         var pagination = "?page=1&pageSize=10";
//         var response = await _client.GetAsync($"/api/v1/ConstitutionArticle/pagedList{pagination}");

//         response.EnsureSuccessStatusCode();

//         var result = await response.Content.ReadAsStringAsync();
        
//         Assert.NotNull(result);
//         Assert.Contains("Page", result);  
//         Assert.Contains("PageSize", result);  
//     }

//     [Fact]
//     public async Task GetConstitutionArticlesByChapterNumber_ReturnsOkWithChapterData()
//     {
//         int chapterNumber = 1;  
//         var response = await _client.GetAsync($"/api/v1/ConstitutionArticle/byChapterNumber/{chapterNumber}");

//         response.EnsureSuccessStatusCode();

//         var result = await response.Content.ReadAsStringAsync();
        
//         Assert.NotNull(result);
//         Assert.Contains("ChapterNumber", result);  
//     }
// }
