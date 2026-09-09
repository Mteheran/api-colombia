using System.Net;
using System.Text.Json;
using api.Tests.Infrastructure;
using api.Tests.TestData;

namespace api.Tests.Integration.Resources;

/// <summary>
/// The nested routes — City's three, Department's four, and the per-parent filters on
/// TypicalDish, IntangibleHeritage, TraditionalFairAndFestival, PostalCode, UrbanCenter and
/// CategoryNaturalArea. Several of these were only ever touched sideways from the child
/// resource's own test file, and their id guards were untested.
/// </summary>
[Collection(SharedApiCollection.Name)]
public class SubResourceContractTests(ApiColombiaFactory factory) : IntegrationTestBase(factory)
{
    /// <summary>A nested route, the parent id that has rows, and how many rows that parent owns.</summary>
    public static TheoryData<string, int, int> NestedRoutes() => new()
    {
        // City's children — Medellín (1) owns two of each, Cali (20) owns one.
        { "/api/v1/City/{0}/highereducationinstitutions", 1, 2 },
        { "/api/v1/City/{0}/televisionchannels", 1, 2 },
        { "/api/v1/City/{0}/volcanoes", 1, 2 },

        // Department's children — Antioquia (1).
        { "/api/v1/Department/{0}/cities", 1, 1 },
        { "/api/v1/Department/{0}/naturalareas", 1, NaturalAreaSeed.TotalRows },
        { "/api/v1/Department/{0}/touristicattractions", 1, TouristAttractionSeed.TotalRows },
        { "/api/v1/Department/{0}/volcanoes", 1, 2 },

        // Per-parent filters that live on the child resource's own group.
        { "/api/v1/TypicalDish/{0}/department", 1, 2 },
        { "/api/v1/IntangibleHeritage/{0}/department", 1, 2 },
        { "/api/v1/TraditionalFairAndFestival/{0}/city", 1, 2 },
        { "/api/v1/PostalCode/city/{0}", 1, 2 },
        { "/api/v1/UrbanCenter/city/{0}", 1, 2 },
    };

    /// <summary>Nested routes that guard the parent id — every one except PostalCode's.</summary>
    public static TheoryData<string> GuardedRoutes() =>
    [
        .. NestedRoutes()
            .Select(row => (string)row[0])
            .Where(route => !route.StartsWith("/api/v1/PostalCode", StringComparison.Ordinal))
    ];

    [Theory]
    [MemberData(nameof(NestedRoutes))]
    public async Task NestedRoute_ReturnsTheParentsRows(string template, int parentId, int expected)
    {
        var rows = await _client.GetJsonAsync<JsonElement>(string.Format(template, parentId));

        Assert.Equal(JsonValueKind.Array, rows.ValueKind);
        Assert.Equal(expected, rows.GetArrayLength());
    }

    [Theory]
    [MemberData(nameof(GuardedRoutes))]
    public async Task NestedRoute_WithZeroParentId_ReturnsBadRequest(string template) =>
        await _client.AssertBadRequestAsync(string.Format(template, 0));

    [Theory]
    [MemberData(nameof(GuardedRoutes))]
    public async Task NestedRoute_WithNegativeParentId_ReturnsBadRequest(string template) =>
        await _client.AssertBadRequestAsync(string.Format(template, -1));

    /// <summary>
    /// PostalCode's `/city/{cityId}` is the one nested route with no `cityId <= 0` guard, so an
    /// invalid id falls through to the empty-result branch and answers 404 instead of 400.
    /// </summary>
    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public async Task PostalCodeByCity_HasNoIdGuard_AndFallsThroughToNotFound(int cityId) =>
        await _client.AssertStatusAsync($"/api/v1/PostalCode/city/{cityId}", HttpStatusCode.NotFound);

    // ---------- CategoryNaturalArea returns the parent, not a list ----------

    [Fact]
    public async Task CategoryNaturalAreaNaturalAreas_ReturnsTheCategoryWithItsAreasNested()
    {
        var category = await _client.GetJsonAsync<JsonElement>("/api/v1/CategoryNaturalArea/1/NaturalAreas");

        // Unlike every other nested route, this one answers with the parent object.
        Assert.Equal(JsonValueKind.Object, category.ValueKind);
        Assert.Equal(1, category.GetProperty("id").GetInt32());
        Assert.Equal(NaturalAreaSeed.TotalRows, category.GetProperty("naturalAreas").GetArrayLength());
    }

    [Fact]
    public async Task CategoryNaturalAreaNaturalAreas_WithUnknownId_ReturnsNotFound() =>
        await _client.AssertNotFoundAsync("/api/v1/CategoryNaturalArea/9999/NaturalAreas");

    // ---------- Region ----------

    [Fact]
    public async Task RegionDepartments_ReturnsBothDepartments()
    {
        var departments = await _client.GetJsonAsync<JsonElement>("/api/v1/Region/1/departments");

        Assert.Equal(JsonValueKind.Array, departments.ValueKind);
        Assert.Equal(CoreGraph.DepartmentRows, departments.GetArrayLength());
    }

    [Fact]
    public async Task RegionDepartments_WithUnknownId_ReturnsNotFound() =>
        await _client.AssertNotFoundAsync("/api/v1/Region/9999/departments");

    [Theory]
    [InlineData("sortBy=notAProperty&sortDirection=asc")]
    [InlineData("sortBy=Name&sortDirection=sideways")]
    public async Task RegionDepartments_WithInvalidSort_ReturnsBadRequest(string query) =>
        await _client.AssertBadRequestAsync($"/api/v1/Region/1/departments?{query}");

    // ---------- Parents with no children ----------

    [Theory]
    [InlineData("/api/v1/City/20/highereducationinstitutions", 1)]
    [InlineData("/api/v1/City/20/televisionchannels", 1)]
    [InlineData("/api/v1/City/20/volcanoes", 1)]
    [InlineData("/api/v1/Department/10/volcanoes", 1)]
    public async Task NestedRoute_ForTheOtherParent_ReturnsOnlyItsRows(string url, int expected)
    {
        var rows = await _client.GetJsonAsync<JsonElement>(url);

        Assert.Equal(expected, rows.GetArrayLength());
    }
}
