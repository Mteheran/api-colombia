using System.Text.Json;
using api.Tests.Infrastructure;
using api.Tests.TestData;

namespace api.Tests.Integration.Resources;

/// <summary>
/// Regression tests for defects the coverage work uncovered. Each one failed before its fix.
/// </summary>
[Collection(SharedApiCollection.Name)]
public class FixedDefectTests(ApiColombiaFactory factory) : IntegrationTestBase(factory)
{
    /// <summary>
    /// InvasiveSpecie's pagedList was the only paged route that never called ApplySorting: it
    /// accepted sortBy/sortDir and silently returned rows in insertion order.
    /// </summary>
    [Theory]
    [InlineData("asc")]
    [InlineData("desc")]
    public async Task InvasiveSpeciePagedList_HonoursSorting(string direction)
    {
        var envelope = await _client.GetJsonAsync<JsonElement>(
            $"/api/v1/InvasiveSpecie/pagedList?page=1&pagesize=10&sortBy=Name&sortDirection={direction}");

        var names = envelope.GetProperty("data").EnumerateArray()
            .Select(r => r.GetProperty("name").GetString()!).ToList();

        var expected = direction == "asc"
            ? names.OrderBy(n => n, StringComparer.CurrentCulture)
            : names.OrderByDescending(n => n, StringComparer.CurrentCulture);

        Assert.Equal(expected.ToList(), names);
    }

    [Fact]
    public async Task InvasiveSpeciePagedList_RejectsAnUnknownSortField() =>
        await _client.AssertBadRequestAsync(
            "/api/v1/InvasiveSpecie/pagedList?page=1&pagesize=10&sortBy=notAProperty&sortDirection=asc");

    /// <summary>
    /// The TraditionalFairAndFestival list validated the sort parameters and then discarded them
    /// with a hard OrderBy(p => p.Id).
    /// </summary>
    [Fact]
    public async Task TraditionalFairAndFestivalList_HonoursSorting()
    {
        var rows = await _client.GetJsonAsync<JsonElement>(
            "/api/v1/TraditionalFairAndFestival?sortBy=Name&sortDirection=desc");

        var names = rows.EnumerateArray().Select(r => r.GetProperty("name").GetString()!).ToList();

        Assert.Equal(names.OrderByDescending(n => n, StringComparer.CurrentCulture).ToList(), names);
    }

    /// <summary>
    /// The list endpoint's Include(City) inner-joined away any festival whose CityId did not
    /// resolve, while pagedList still counted it — so the two disagreed on how many rows exist.
    /// </summary>
    [Fact]
    public async Task TraditionalFairAndFestival_ListAndPagedList_AgreeOnTheRowCount()
    {
        var rows = await _client.GetJsonAsync<JsonElement>("/api/v1/TraditionalFairAndFestival");
        var envelope = await _client.GetJsonAsync<JsonElement>(
            "/api/v1/TraditionalFairAndFestival/pagedList?page=1&pagesize=50");

        Assert.Equal(TraditionalFairAndFestivalSeed.TotalRows, rows.GetArrayLength());
        Assert.Equal(rows.GetArrayLength(), envelope.GetProperty("totalRecords").GetInt32());
    }

    /// <summary>
    /// Seven /name/{name} handlers null-checked a list that ToListAsync can never return as null,
    /// so their 404 branch was unreachable. Removing the dead code must not change the answer.
    /// </summary>
    [Theory]
    [InlineData("Department")]
    [InlineData("NativeCommunity")]
    [InlineData("IndigenousReservation")]
    [InlineData("Radio")]
    [InlineData("TypicalDish")]
    [InlineData("IntangibleHeritage")]
    [InlineData("TraditionalFairAndFestival")]
    public async Task ByName_WithNoMatch_StillReturnsAnEmptyArray(string route)
    {
        var rows = await _client.GetJsonAsync<JsonElement>($"/api/v1/{route}/name/zzzzzzzzzzzz");

        Assert.Equal(JsonValueKind.Array, rows.ValueKind);
        Assert.Equal(0, rows.GetArrayLength());
    }

    /// <summary>Swagger declared several endpoints as returning City rather than their own type.</summary>
    [Theory]
    [InlineData("/api/v1/Radio/{id}", "Radio")]
    [InlineData("/api/v1/NativeCommunity/{id}", "NativeCommunity")]
    [InlineData("/api/v1/InvasiveSpecie/pagedList", "InvasiveSpecie")]
    public async Task SwaggerDeclaresTheCorrectResponseType(string path, string expectedType)
    {
        var document = await _client.GetJsonAsync<JsonElement>("/swagger/v1/swagger.json");
        var schema = document.GetProperty("paths").GetProperty(path)
            .GetProperty("get").GetProperty("responses").GetProperty("200")
            .GetProperty("content").GetProperty("application/json").GetProperty("schema");

        Assert.Contains(expectedType, schema.ToString());
    }
}
