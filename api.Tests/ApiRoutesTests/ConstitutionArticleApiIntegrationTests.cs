using System.Net.Http;
using System.Threading.Tasks;
using Xunit;
using Microsoft.AspNetCore.Mvc.Testing;

public class ConstitutionArticleApiIntegrationTests : IClassFixture<CustomWebApplicationFactory>, IDisposable
{
    private readonly HttpClient _client;

    public ConstitutionArticleApiIntegrationTests()
    {
       _client = new CustomWebApplicationFactory().CreateClient(); 
    }

    public void Dispose()
    {
        _client.Dispose();
    }
    
    [Fact]
    public async Task GetConstitutionArticles_ReturnsOkWithExpectedData()
    {   
        var response = await _client.GetAsync("/api/v1/ConstitutionArticle");
        
        response.EnsureSuccessStatusCode();
        
        var result = await response.Content.ReadAsStringAsync();
        
        Assert.NotNull(result);    
        Assert.False(string.IsNullOrEmpty(result));  
    }

    [Fact]
    public async Task GetConstitutionArticleById_ReturnsOkWithConstitutionArticleData()
    {
        int constitutionArticleId = 1;  
        var response = await _client.GetAsync($"/api/v1/ConstitutionArticle/{constitutionArticleId}");

        response.EnsureSuccessStatusCode();

        var result = await response.Content.ReadAsStringAsync();
        
        Assert.NotNull(result);
        Assert.Contains("id", result);  
    }

    [Fact]
    public async Task GetConstitutionArticlesWithSorting_ReturnsOkWithSortedData()
    {
        string sortBy = "titleNumber"; 
        string sortDirection = "asc";  

        var response = await _client.GetAsync($"/api/v1/ConstitutionArticle?sortBy={sortBy}&sortDirection={sortDirection}");

        response.EnsureSuccessStatusCode();

        var result = await response.Content.ReadAsStringAsync();
        
        Assert.NotNull(result);
        Assert.Contains("titleNumber", result);  
    }

    [Fact]
    public async Task SearchConstitutionArticles_ReturnsOkWithFilteredData()
    {
        string keyword = "ADMINISTRATIVA";
        var response = await _client.GetAsync($"/api/v1/ConstitutionArticle/search/{keyword}");

        response.EnsureSuccessStatusCode();

        var result = await response.Content.ReadAsStringAsync();
        
        Assert.NotNull(result);
        Assert.Contains("id", result);  
    }

    [Fact]
    public async Task GetPagedConstitutionArticles_ReturnsOkWithPagedData()
    {
        var pagination = "?page=1&pageSize=10";
        var response = await _client.GetAsync($"/api/v1/ConstitutionArticle/pagedList{pagination}");

        response.EnsureSuccessStatusCode();

        var result = await response.Content.ReadAsStringAsync();
        
        Assert.NotNull(result);
        Assert.Contains("page", result);  
        Assert.Contains("pageSize", result);  
    }

    [Fact]
    public async Task GetConstitutionArticlesByChapterNumber_ReturnsOkWithChapterData()
    {
        int chapterNumber = 1;  
        var response = await _client.GetAsync($"/api/v1/ConstitutionArticle/byChapterNumber/{chapterNumber}");

        response.EnsureSuccessStatusCode();

        var result = await response.Content.ReadAsStringAsync();
        
        Assert.NotNull(result);
        Assert.Contains("titleNumber", result);  
    }
}
