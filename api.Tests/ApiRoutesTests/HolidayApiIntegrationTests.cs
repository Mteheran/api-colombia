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

    private const string ChiquinquiraName = "Día de Nuestra Señora del Rosario de Chiquinquirá";

    [Theory]
    [InlineData(2024)]
    [InlineData(2025)]
    public async Task GetHolidays_Before2026_DoesNotIncludeChiquinquira(int year)
    {
        var response = await _client.GetAsync($"/api/v1/Holiday/year/{year}");
        response.EnsureSuccessStatusCode();

        var result = await response.Content.ReadFromJsonAsync<List<Holiday>>();
        Assert.NotNull(result);
        Assert.DoesNotContain(result!, h => h.Name == ChiquinquiraName);
    }

    [Theory]
    // 2026: July 9 is Thursday (weekday) -> Monday of the following week, July 13
    [InlineData(2026, 7, 13)]
    // 2027: July 9 is Friday (weekday) -> Monday of the following week, July 12
    [InlineData(2027, 7, 12)]
    // 2028: July 9 is Sunday -> stays on July 9
    [InlineData(2028, 7, 9)]
    // 2033: July 9 is Saturday -> stays on July 9
    [InlineData(2033, 7, 9)]
    // 2029: July 9 is Monday (weekday) -> Monday of the following week, July 16
    [InlineData(2029, 7, 16)]
    public async Task GetHolidays_From2026_IncludesChiquinquiraOnExpectedDate(int year, int expectedMonth, int expectedDay)
    {
        var response = await _client.GetAsync($"/api/v1/Holiday/year/{year}");
        response.EnsureSuccessStatusCode();

        var result = await response.Content.ReadFromJsonAsync<List<Holiday>>();
        Assert.NotNull(result);

        var chiquinquira = Assert.Single(result!, h => h.Name == ChiquinquiraName);
        Assert.Equal(new DateTime(year, expectedMonth, expectedDay), chiquinquira.Date);
    }
}
