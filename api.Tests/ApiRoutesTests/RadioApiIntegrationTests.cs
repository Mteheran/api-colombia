using System.Net.Http;
using System.Threading.Tasks;
using Xunit;
using Microsoft.AspNetCore.Mvc.Testing;
using System.Net.Http.Json;
using api.Models;
using api.Utils;

public class RadioApiIntegrationTests : IClassFixture<CustomWebApplicationFactory>
{
    private readonly HttpClient _client;

    public RadioApiIntegrationTests(CustomWebApplicationFactory factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task GetRadios_ReturnsOkWithExpectedData()
    {
        var response = await _client.GetAsync("/api/v1/Radio");

        response.EnsureSuccessStatusCode();

        var result = await response.Content.ReadFromJsonAsync<List<Radio>>();

        Assert.NotNull(result);
        Assert.Equal(3, result.Count);
    }

    [Fact]
    public async Task GetRadioById_ReturnsOkWithRadioData()
    {
        int radioId = 1;  
        var response = await _client.GetAsync($"/api/v1/Radio/{radioId}");

        response.EnsureSuccessStatusCode();

        var result = await response.Content.ReadFromJsonAsync<Radio>();

        Assert.NotNull(result);
        Assert.Equal(radioId, result.Id);
    }

    [Fact]
    public async Task GetRadioByName_ReturnsOkWithRadioData()
    {
        string radioName = "Sample Radio";  
        var response = await _client.GetAsync($"/api/v1/Radio/name/{radioName}");

        response.EnsureSuccessStatusCode();

        var result = await response.Content.ReadFromJsonAsync<List<Radio>>();

        Assert.NotNull(result);
        Assert.Single(result);
        Assert.Contains(result, r => r.Name == radioName);
    }

    [Fact]
    public async Task SearchRadios_ReturnsOkWithFilteredData()
    {
        string searchKeyword = "anoth";  
        var response = await _client.GetAsync($"/api/v1/Radio/search/{searchKeyword}");

        response.EnsureSuccessStatusCode();

        var result = await response.Content.ReadFromJsonAsync<List<Radio>>();

        Assert.NotNull(result);
        Assert.NotEmpty(result);
        Assert.Single(result);
        Assert.All(result, r => Assert.Contains(searchKeyword, r.Name, StringComparison.OrdinalIgnoreCase));
    }

    [Fact]
    public async Task GetPagedRadios_ReturnsOkWithPagedData()
    {
        int page = 1;
        int pageSize = 1;

        var response = await _client.GetAsync($"/api/v1/Radio/pagedList?page={page}&pageSize={pageSize}");

        response.EnsureSuccessStatusCode();

        var result = await response.Content.ReadFromJsonAsync<PaginationResponseModel<Radio>>();

        Assert.NotNull(result);
        Assert.Equal(page, result.Page);
        Assert.Equal(pageSize, result.PageSize);
        Assert.Equal(3, result.TotalRecords);
    }
}
