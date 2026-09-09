using System.Net.Http.Json;
using api.Models;
using api.Utils;

using api.Tests.Infrastructure;

namespace api.Tests.Integration.Resources;

[Collection(SharedApiCollection.Name)]
public class TelevisionChannelApiIntegrationTests(ApiColombiaFactory factory) : IntegrationTestBase(factory)
{
    [Fact]
    public async Task GetTelevisionChannels_ReturnsOkWithExpectedData()
    {
        var response = await _client.GetAsync("/api/v1/TelevisionChannel");

        response.EnsureSuccessStatusCode();

        var result = await response.Content.ReadFromJsonAsync<List<TelevisionChannel>>();

        Assert.NotNull(result);
        Assert.NotEmpty(result);
        Assert.Equal(3, result.Count);
    }

    [Fact]
    public async Task GetTelevisionChannelById_ReturnsOkWithData()
    {
        int id = 1;
        var response = await _client.GetAsync($"/api/v1/TelevisionChannel/{id}");

        response.EnsureSuccessStatusCode();

        var result = await response.Content.ReadFromJsonAsync<TelevisionChannel>();

        Assert.NotNull(result);
        Assert.Equal(id, result.Id);
        Assert.Equal("Sample TV Channel", result.Name);
        Assert.True(result.IsActive);
    }

    [Fact]
    public async Task GetTelevisionChannelById_ReturnsBadRequest()
    {
        int id = 0;
        var response = await _client.GetAsync($"/api/v1/TelevisionChannel/{id}");

        Assert.False(response.IsSuccessStatusCode);
        Assert.Equal(System.Net.HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task GetTelevisionChannelById_ReturnsNotFound()
    {
        int id = 9999;
        var response = await _client.GetAsync($"/api/v1/TelevisionChannel/{id}");

        Assert.Equal(System.Net.HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task GetTelevisionChannelByName_ReturnsOkWithExpectedData()
    {
        string name = "Sample TV Channel";
        var response = await _client.GetAsync($"/api/v1/TelevisionChannel/name/{name}");

        response.EnsureSuccessStatusCode();

        var result = await response.Content.ReadFromJsonAsync<List<TelevisionChannel>>();

        Assert.NotNull(result);
        Assert.Single(result);
        Assert.Equal("Sample TV Channel", result[0].Name);
    }

    [Fact]
    public async Task SearchTelevisionChannels_ReturnsOkWithFilteredResults()
    {
        string keyword = "Another";
        var response = await _client.GetAsync($"/api/v1/TelevisionChannel/search/{keyword}");

        response.EnsureSuccessStatusCode();

        var result = await response.Content.ReadFromJsonAsync<List<TelevisionChannel>>();

        Assert.NotNull(result);
        Assert.Single(result);
        Assert.Contains(keyword, result[0].Name, StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public async Task SearchTelevisionChannels_WithShortKeyword_ReturnsOkWithFilteredResults()
    {
        string keyword = "RCN";
        var response = await _client.GetAsync($"/api/v1/TelevisionChannel/search/{keyword}");

        response.EnsureSuccessStatusCode();

        var result = await response.Content.ReadFromJsonAsync<List<TelevisionChannel>>();

        Assert.NotNull(result);
        Assert.Single(result);
        Assert.Equal("Canal RCN", result[0].Name);
    }

    [Fact]
    public async Task SearchTelevisionChannels_IsCaseInsensitive()
    {
        string keyword = "rcn";
        var response = await _client.GetAsync($"/api/v1/TelevisionChannel/search/{keyword}");

        response.EnsureSuccessStatusCode();

        var result = await response.Content.ReadFromJsonAsync<List<TelevisionChannel>>();

        Assert.NotNull(result);
        Assert.Single(result);
        Assert.Equal("Canal RCN", result[0].Name);
    }

    [Fact]
    public async Task SearchTelevisionChannels_WithoutMatches_ReturnsEmptyList()
    {
        string keyword = "NotAChannelName";
        var response = await _client.GetAsync($"/api/v1/TelevisionChannel/search/{keyword}");

        response.EnsureSuccessStatusCode();

        var result = await response.Content.ReadFromJsonAsync<List<TelevisionChannel>>();

        Assert.NotNull(result);
        Assert.Empty(result);
    }

    [Fact]
    public async Task GetPagedTelevisionChannels_ReturnsOkWithPagedData()
    {
        int page = 1;
        int pageSize = 2;

        var response = await _client.GetAsync($"/api/v1/TelevisionChannel/pagedList?page={page}&pageSize={pageSize}");

        response.EnsureSuccessStatusCode();

        var result = await response.Content.ReadFromJsonAsync<PaginationResponseModel<TelevisionChannel>>();

        Assert.NotNull(result);
        Assert.Equal(pageSize, result.PageSize);
        Assert.Equal(page, result.Page);
        Assert.Equal(pageSize, result.Data.Count);
        Assert.Equal(3, result.TotalRecords);
    }

    [Fact]
    public async Task GetTelevisionChannelsByCity_ReturnsOkWithExpectedData()
    {
        int cityId = 1;
        var response = await _client.GetAsync($"/api/v1/City/{cityId}/televisionchannels");

        response.EnsureSuccessStatusCode();

        var result = await response.Content.ReadFromJsonAsync<List<TelevisionChannel>>();

        Assert.NotNull(result);
        Assert.Equal(2, result.Count);
        Assert.All(result, c => Assert.Equal(cityId, c.CityId));
    }
}
