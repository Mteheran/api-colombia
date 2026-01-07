using System.Net;
using System.Net.Http.Json;
using api.Models;

namespace api.Tests.ApiRoutesTests;

public class HolidayApiIntegrationTests : IClassFixture<CustomWebApplicationFactory>
{
    private readonly HttpClient _client;

    public HolidayApiIntegrationTests(CustomWebApplicationFactory factory)
    {
        factory.ResetDatabase();
        _client = factory.CreateClient();
    }

    [Theory]
    [InlineData(2025)]
    [InlineData(2024)]
    public async Task GetHolidays_ByYear_ReturnsOkAndSameYear(int year)
    {
        var response = await _client.GetAsync($"/api/v1/Holiday/year/{year}");
        response.EnsureSuccessStatusCode();

        var result = await response.Content.ReadFromJsonAsync<List<Holiday>>();
        Assert.NotNull(result);
        Assert.NotEmpty(result);
        Assert.All(result!, h => Assert.Equal(year, h.Date.Year));
    }

    [Theory]
    [InlineData(-1)] // less than DateTime.MinValue.Year (1)
    [InlineData(10000)] // greater than DateTime.MaxValue.Year (9999)
    public async Task GetHolidays_ByYear_InvalidYear_ReturnsBadRequest(int invalidYear)
    {
        var response = await _client.GetAsync($"/api/v1/Holiday/year/{invalidYear}");
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Theory]
    [InlineData(2025, 1)] // January should include New Year
    [InlineData(2025, 12)] // December should include Christmas and Immaculate Conception
    public async Task GetHolidays_ByYearAndMonth_ReturnsOnlyMonth(int year, int month)
    {
        var response = await _client.GetAsync($"/api/v1/Holiday/year/{year}/month/{month}");
        response.EnsureSuccessStatusCode();

        var result = await response.Content.ReadFromJsonAsync<List<Holiday>>();
        Assert.NotNull(result);
        Assert.NotEmpty(result);
        Assert.All(result!, h => Assert.Equal(month, h.Date.Month));
        Assert.All(result!, h => Assert.Equal(year, h.Date.Year));
    }

    [Theory]
    [InlineData(2025, 0)]
    [InlineData(2025, 13)]
    public async Task GetHolidays_ByYearAndMonth_InvalidMonth_ReturnsBadRequest(int year, int invalidMonth)
    {
        var response = await _client.GetAsync($"/api/v1/Holiday/year/{year}/month/{invalidMonth}");
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }
}
