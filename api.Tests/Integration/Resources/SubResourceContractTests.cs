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

    /// <summary>Every nested route guards the parent id and answers 400 for a non-positive one.</summary>
    public static TheoryData<string> GuardedRoutes() =>
    [
        .. NestedRoutes().Select(row => (string)row[0])
    ];

    /// <summary>Nested routes that accept <c>?sortBy=&amp;sortDirection=</c>, with a property to sort on.</summary>
    public static TheoryData<string, string> SortableRoutes() => new()
    {
        { "/api/v1/City/1/highereducationinstitutions", "Name" },
        { "/api/v1/City/1/televisionchannels", "Name" },
        { "/api/v1/City/1/volcanoes", "Name" },
        { "/api/v1/Department/1/cities", "Name" },
        { "/api/v1/Department/1/naturalareas", "Name" },
        { "/api/v1/Department/1/touristicattractions", "Name" },
        { "/api/v1/Department/1/volcanoes", "Name" },
        { "/api/v1/TypicalDish/1/department", "Name" },
        { "/api/v1/IntangibleHeritage/1/department", "Name" },
        { "/api/v1/TraditionalFairAndFestival/1/city", "Name" },
    };

    /// <summary>
    /// What each nested route answers for a parent id that is valid but owns nothing. Like the
    /// top-level contracts, this records what the API does TODAY — it is not consistent: some
    /// routes answer 404, others 200 with an empty array.
    /// </summary>
    public static TheoryData<string, HttpStatusCode> UnknownParentRoutes() => new()
    {
        { "/api/v1/City/9999/highereducationinstitutions", HttpStatusCode.OK },
        { "/api/v1/City/9999/televisionchannels", HttpStatusCode.OK },
        { "/api/v1/City/9999/volcanoes", HttpStatusCode.OK },
        { "/api/v1/Department/9999/cities", HttpStatusCode.OK },
        { "/api/v1/Department/9999/volcanoes", HttpStatusCode.OK },
        { "/api/v1/Department/9999/naturalareas", HttpStatusCode.NotFound },
        { "/api/v1/Department/9999/touristicattractions", HttpStatusCode.NotFound },
        { "/api/v1/TypicalDish/9999/department", HttpStatusCode.NotFound },
        { "/api/v1/IntangibleHeritage/9999/department", HttpStatusCode.NotFound },
        { "/api/v1/TraditionalFairAndFestival/9999/city", HttpStatusCode.NotFound },
        { "/api/v1/PostalCode/city/9999", HttpStatusCode.NotFound },
        { "/api/v1/UrbanCenter/city/9999", HttpStatusCode.NotFound },
    };

    [Theory]
    [MemberData(nameof(SortableRoutes))]
    public async Task NestedRoute_WithUnknownSortBy_ReturnsBadRequest(string url, string _) =>
        await _client.AssertBadRequestAsync($"{url}?sortBy=notAProperty&sortDirection=asc");

    [Theory]
    [MemberData(nameof(SortableRoutes))]
    public async Task NestedRoute_WithUnknownSortDirection_ReturnsBadRequest(string url, string sortBy) =>
        await _client.AssertBadRequestAsync($"{url}?sortBy={sortBy}&sortDirection=sideways");

    [Theory]
    [MemberData(nameof(SortableRoutes))]
    public async Task NestedRoute_SortedDescending_ReturnsRowsInOrder(string url, string sortBy)
    {
        var rows = await _client.GetJsonAsync<JsonElement>($"{url}?sortBy={sortBy}&sortDirection=desc");

        var property = char.ToLowerInvariant(sortBy[0]) + sortBy[1..];
        var values = rows.EnumerateArray().Select(r => r.GetProperty(property).GetString() ?? string.Empty).ToList();

        Assert.NotEmpty(values);
        Assert.Equal(values.OrderByDescending(v => v, StringComparer.CurrentCulture).ToList(), values);
    }

    [Theory]
    [MemberData(nameof(UnknownParentRoutes))]
    public async Task NestedRoute_ForAParentWithNoRows_ReturnsTheDocumentedContract(string url, HttpStatusCode expected)
    {
        var response = await _client.GetAsync(url);

        Assert.Equal(expected, response.StatusCode);
        if (expected == HttpStatusCode.OK)
        {
            var rows = JsonDocument.Parse(await response.Content.ReadAsStringAsync()).RootElement;
            Assert.Equal(0, rows.GetArrayLength());
        }
    }

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
    /// PostalCode's `/city/{cityId}` used to be the one nested route with no `cityId <= 0` guard,
    /// so an invalid id fell through to the empty-result branch and answered 404 instead of 400.
    /// </summary>
    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public async Task PostalCodeByCity_GuardsTheCityId(int cityId) =>
        await _client.AssertBadRequestAsync($"/api/v1/PostalCode/city/{cityId}");

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

    // ---------- Id guards on the routes that return the parent ----------

    [Theory]
    [InlineData("/api/v1/Region/0/departments")]
    [InlineData("/api/v1/Region/-1/departments")]
    [InlineData("/api/v1/CategoryNaturalArea/0/NaturalAreas")]
    [InlineData("/api/v1/CategoryNaturalArea/-1/NaturalAreas")]
    public async Task ParentReturningRoute_WithNonPositiveId_ReturnsBadRequest(string url) =>
        await _client.AssertBadRequestAsync(url);

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
