using System.Net;
using System.Text.Json;
using api.Mcp;
using api.Tests.Infrastructure;
using Microsoft.Extensions.DependencyInjection;

namespace api.Tests.Integration.Platform;

/// <summary>
/// A host whose database exists but holds no rows — what a fresh deployment answers before the
/// seed scripts run. Several handlers have a branch that only fires on an empty table (Country's
/// 404, the "source table is empty" check in three /search routes, the MCP country tool's error),
/// and the shared seeded host can never reach them.
/// </summary>
public class EmptyDatabaseTests(EmptyDatabaseFactory factory) : IClassFixture<EmptyDatabaseFactory>
{
    private readonly HttpClient _client = factory.CreateClient();

    public static TheoryData<string> ListRoutes() =>
    [
        "Airport", "City", "Department", "Region", "President", "TouristicAttraction",
        "CategoryNaturalArea", "NaturalArea", "Map", "InvasiveSpecie", "NativeCommunity",
        "IndigenousReservation", "ConstitutionArticle", "Radio", "TypicalDish",
        "TraditionalFairAndFestival", "IntangibleHeritage", "HeritageCity", "PostalCode",
        "UrbanCenter", "HigherEducationInstitution", "TelevisionChannel", "Volcano",
    ];

    [Fact]
    public async Task Country_WithNoRow_ReturnsNotFound() =>
        await _client.AssertNotFoundAsync("/api/v1/Country/Colombia");

    [Theory]
    [MemberData(nameof(ListRoutes))]
    public async Task List_OnAnEmptyTable_ReturnsAnEmptyArray(string route)
    {
        var rows = await _client.GetJsonAsync<JsonElement>($"/api/v1/{route}");

        Assert.Equal(JsonValueKind.Array, rows.ValueKind);
        Assert.Equal(0, rows.GetArrayLength());
    }

    /// <summary>
    /// These three check whether the TABLE is empty rather than whether anything matched, so a
    /// no-match search on a seeded host answers 200 — only an empty table produces their 404.
    /// </summary>
    [Theory]
    [InlineData("TypicalDish")]
    [InlineData("TraditionalFairAndFestival")]
    [InlineData("IntangibleHeritage")]
    public async Task Search_OnAnEmptyTable_ReturnsNotFound(string route) =>
        await _client.AssertNotFoundAsync($"/api/v1/{route}/search/anything");

    [Fact]
    public void McpCountryTool_WithNoRow_ReturnsAToolError()
    {
        using var scope = factory.Services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<DBContext>();

        var result = JsonSerializer.SerializeToElement(DataTools.GetCountryInfo(db));

        Assert.Equal("No country information available.", result.GetProperty("error").GetString());
    }

    [Fact]
    public async Task Holidays_DoNotDependOnTheDatabase()
    {
        // Holidays are computed, not stored, so they still answer on an empty database.
        var holidays = await _client.GetJsonAsync<JsonElement>("/api/v1/Holiday/year/2026");

        Assert.True(holidays.GetArrayLength() > 0);
    }

    [Fact]
    public async Task PagedList_OnAnEmptyTable_ReturnsAnEmptyEnvelope()
    {
        var response = await _client.GetAsync("/api/v1/City/pagedList?page=1&pagesize=10");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var envelope = JsonDocument.Parse(await response.Content.ReadAsStringAsync()).RootElement;
        Assert.Equal(0, envelope.GetProperty("totalRecords").GetInt32());
        Assert.Equal(0, envelope.GetProperty("data").GetArrayLength());
    }
}
