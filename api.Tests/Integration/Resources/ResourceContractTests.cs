using System.Net;
using System.Text.Json;
using api.Tests.Infrastructure;
using api.Tests.TestData;

namespace api.Tests.Integration.Resources;

/// <summary>
/// The scenario matrix every list-shaped resource must satisfy, applied uniformly instead of being
/// re-typed (and quietly skipped) in 25 near-identical files. Resource-specific semantics stay in
/// the per-resource classes; what lives here is the shape contract: sorting validation, id guards,
/// pagination envelope and the empty-result behaviour.
///
/// <see cref="Contracts"/> doubles as the inventory of how inconsistent the API currently is about
/// empty results — some endpoints answer 404, others 200 with an empty array. Each row records what
/// the endpoint does TODAY, so changing a contract is a visible edit here rather than a surprise.
/// </summary>
[Collection(SharedApiCollection.Name)]
public class ResourceContractTests(ApiColombiaFactory factory) : IntegrationTestBase(factory)
{
    private const string NoMatch = "zzzzzzzzzzzz";
    private const int UnknownId = 9999;
    private const HttpStatusCode NotFound = HttpStatusCode.NotFound;

    public sealed record Contract
    {
        public required string Route { get; init; }
        public required int TotalRows { get; init; }

        /// <summary>Property fed to <c>?sortBy=</c>; must exist on the model.</summary>
        public string SortBy { get; init; } = "Name";

        /// <summary>Lookup segment for the "by exact value" route: "name", "code", or null when absent.</summary>
        public string? LookupSegment { get; init; } = "name";

        /// <summary>What the lookup route answers when nothing matches.</summary>
        public HttpStatusCode LookupEmpty { get; init; } = HttpStatusCode.OK;

        public bool HasSearch { get; init; } = true;

        /// <summary>What <c>/search/{keyword}</c> answers when nothing matches.</summary>
        public HttpStatusCode SearchEmpty { get; init; } = HttpStatusCode.OK;

        public bool HasPagedList { get; init; } = true;

        /// <summary>What <c>/pagedList</c> answers for a page past the end.</summary>
        public HttpStatusCode PagedOutOfRange { get; init; } = HttpStatusCode.OK;
    }

    // Every value below is what the endpoint ACTUALLY does today, verified against the running API
    // rather than read off the source: several routes contain a NotFound branch that is unreachable
    // (the handler null-checks a list that ToListAsync can never return as null), so the source
    // reads 404 while the endpoint answers 200 with an empty array.
    private static readonly Dictionary<string, Contract> Contracts = new Contract[]
    {
        new() { Route = "Airport",       TotalRows = AirportSeed.TotalRows },
        new() { Route = "City",          TotalRows = CoreGraph.CityRows },
        new() { Route = "HeritageCity",  TotalRows = HeritageCitySeed.TotalRows },
        new() { Route = "Volcano",       TotalRows = VolcanoSeed.TotalRows },
        new() { Route = "TelevisionChannel", TotalRows = TelevisionChannelSeed.TotalRows },
        new() { Route = "HigherEducationInstitution", TotalRows = HigherEducationInstitutionSeed.TotalRows },

        new() { Route = "Department",    TotalRows = CoreGraph.DepartmentRows, SearchEmpty = NotFound, PagedOutOfRange = NotFound },
        new() { Route = "Radio",         TotalRows = RadioSeed.TotalRows, SearchEmpty = NotFound, PagedOutOfRange = NotFound },
        new() { Route = "NativeCommunity", TotalRows = NativeCommunitySeed.TotalRows, SearchEmpty = NotFound, PagedOutOfRange = NotFound },
        new() { Route = "IndigenousReservation", TotalRows = IndigenousReservationSeed.TotalRows, SearchEmpty = NotFound, PagedOutOfRange = NotFound },

        new() { Route = "NaturalArea",   TotalRows = NaturalAreaSeed.TotalRows, PagedOutOfRange = NotFound },
        new() { Route = "TouristicAttraction", TotalRows = TouristAttractionSeed.TotalRows, PagedOutOfRange = NotFound },
        new() { Route = "InvasiveSpecie", TotalRows = InvasiveSpecieSeed.TotalRows, PagedOutOfRange = NotFound },
        new() { Route = "TypicalDish",   TotalRows = TypicalDishSeed.TotalRows, PagedOutOfRange = NotFound },
        new() { Route = "IntangibleHeritage", TotalRows = IntangibleHeritageSeed.TotalRows, PagedOutOfRange = NotFound },
        new() { Route = "TraditionalFairAndFestival", TotalRows = TraditionalFairAndFestivalSeed.TotalRows, PagedOutOfRange = NotFound },

        // President is the only resource whose /name/{name} answers 404 when nothing matches.
        new() { Route = "President",     TotalRows = PresidentSeed.TotalRows, LookupEmpty = NotFound, SearchEmpty = NotFound, PagedOutOfRange = NotFound },

        new() { Route = "ConstitutionArticle", TotalRows = ConstitutionArticleSeed.TotalRows, SortBy = "Title", LookupSegment = null, SearchEmpty = NotFound, PagedOutOfRange = NotFound },
        new() { Route = "PostalCode",    TotalRows = PostalCodeSeed.TotalRows, SortBy = "Code", LookupSegment = "code", LookupEmpty = NotFound },
        new() { Route = "UrbanCenter",   TotalRows = UrbanCenterSeed.TotalRows, LookupSegment = "code", LookupEmpty = NotFound, SearchEmpty = NotFound, PagedOutOfRange = NotFound },

        // List + by-id only: no name, search or pagination routes exist.
        new() { Route = "Region",              TotalRows = CoreGraph.RegionRows, LookupSegment = null, HasSearch = false, HasPagedList = false },
        new() { Route = "CategoryNaturalArea", TotalRows = CategoryNaturalAreaSeed.TotalRows, LookupSegment = null, HasSearch = false, HasPagedList = false },
        new() { Route = "Map",                 TotalRows = MapSeed.TotalRows, LookupSegment = null, HasSearch = false, HasPagedList = false },
    }.ToDictionary(c => c.Route);

    public static TheoryData<string> AllResources() => [.. Contracts.Keys];

    public static TheoryData<string> Searchable() =>
        [.. Contracts.Values.Where(c => c.HasSearch).Select(c => c.Route)];

    public static TheoryData<string> Paged() =>
        [.. Contracts.Values.Where(c => c.HasPagedList).Select(c => c.Route)];

    public static TheoryData<string> WithLookup() =>
        [.. Contracts.Values.Where(c => c.LookupSegment is not null).Select(c => c.Route)];

    private static string Url(string route) => $"/api/v1/{route}";

    // ---------- GET "" ----------

    [Theory]
    [MemberData(nameof(AllResources))]
    public async Task List_ReturnsEverySeededRow(string route)
    {
        var contract = Contracts[route];

        var rows = await _client.GetJsonAsync<JsonElement>(Url(route));

        Assert.Equal(JsonValueKind.Array, rows.ValueKind);
        Assert.Equal(contract.TotalRows, rows.GetArrayLength());
    }

    [Theory]
    [MemberData(nameof(AllResources))]
    public async Task List_WithUnknownSortBy_ReturnsBadRequest(string route) =>
        await _client.AssertBadRequestAsync($"{Url(route)}?sortBy=notAProperty&sortDirection=asc");

    [Theory]
    [MemberData(nameof(AllResources))]
    public async Task List_WithUnknownSortDirection_ReturnsBadRequest(string route)
    {
        var contract = Contracts[route];

        await _client.AssertBadRequestAsync($"{Url(route)}?sortBy={contract.SortBy}&sortDirection=sideways");
    }

    [Theory]
    [MemberData(nameof(AllResources))]
    public async Task List_SortedAscending_ReturnsRowsInOrder(string route) =>
        await AssertSorted(route, "asc");

    [Theory]
    [MemberData(nameof(AllResources))]
    public async Task List_SortedDescending_ReturnsRowsInOrder(string route) =>
        await AssertSorted(route, "desc");

    private async Task AssertSorted(string route, string direction)
    {
        var contract = Contracts[route];

        var rows = await _client.GetJsonAsync<JsonElement>(
            $"{Url(route)}?sortBy={contract.SortBy}&sortDirection={direction}");

        var values = SortValues(rows, contract.SortBy);
        // ApplySorting builds a plain OrderBy, so the server compares with the default
        // culture-sensitive comparer — ordinal would disagree on the accented names.
        var expected = direction == "asc"
            ? values.OrderBy(v => v, StringComparer.CurrentCulture)
            : values.OrderByDescending(v => v, StringComparer.CurrentCulture);

        Assert.Equal(expected.ToList(), values);
    }

    private static List<string> SortValues(JsonElement rows, string property)
    {
        var camelCase = char.ToLowerInvariant(property[0]) + property[1..];

        return [.. rows.EnumerateArray().Select(r => r.GetProperty(camelCase).GetString() ?? string.Empty)];
    }

    // ---------- GET /{id} ----------

    [Theory]
    [MemberData(nameof(AllResources))]
    public async Task ById_WithZero_ReturnsBadRequest(string route) =>
        await _client.AssertBadRequestAsync($"{Url(route)}/0");

    [Theory]
    [MemberData(nameof(AllResources))]
    public async Task ById_WithNegativeId_ReturnsBadRequest(string route) =>
        await _client.AssertBadRequestAsync($"{Url(route)}/-1");

    [Theory]
    [MemberData(nameof(AllResources))]
    public async Task ById_WithUnknownId_ReturnsNotFound(string route) =>
        await _client.AssertNotFoundAsync($"{Url(route)}/{UnknownId}");

    [Theory]
    [MemberData(nameof(AllResources))]
    public async Task ById_WithSeededId_ReturnsThatRow(string route)
    {
        var row = await _client.GetJsonAsync<JsonElement>($"{Url(route)}/1");

        Assert.Equal(1, row.GetProperty("id").GetInt32());
    }

    // ---------- GET /name|code/{value} ----------

    [Theory]
    [MemberData(nameof(WithLookup))]
    public async Task Lookup_WithNoMatch_ReturnsTheDocumentedContract(string route)
    {
        var contract = Contracts[route];

        await _client.AssertStatusAsync($"{Url(route)}/{contract.LookupSegment}/{NoMatch}", contract.LookupEmpty);
    }

    // ---------- GET /search/{keyword} ----------

    [Theory]
    [MemberData(nameof(Searchable))]
    public async Task Search_WithNoMatch_ReturnsTheDocumentedContract(string route)
    {
        var contract = Contracts[route];

        await _client.AssertStatusAsync($"{Url(route)}/search/{NoMatch}", contract.SearchEmpty);
    }

    // ---------- GET /pagedList ----------

    [Theory]
    [MemberData(nameof(Paged))]
    public async Task PagedList_ReturnsAFullEnvelope(string route)
    {
        var contract = Contracts[route];

        var envelope = await _client.GetJsonAsync<JsonElement>($"{Url(route)}/pagedList?page=1&pagesize=50");

        Assert.Equal(1, envelope.GetProperty("page").GetInt32());
        Assert.Equal(50, envelope.GetProperty("pageSize").GetInt32());
        Assert.Equal(contract.TotalRows, envelope.GetProperty("totalRecords").GetInt32());
        Assert.Equal(contract.TotalRows, envelope.GetProperty("data").GetArrayLength());
        Assert.True(envelope.GetProperty("pageCount").GetInt32() >= 1);
    }

    [Theory]
    [MemberData(nameof(Paged))]
    public async Task PagedList_WithoutPageSize_ReturnsBadRequest(string route) =>
        await _client.AssertBadRequestAsync($"{Url(route)}/pagedList?page=1");

    [Theory]
    [MemberData(nameof(Paged))]
    public async Task PagedList_WithZeroPageSize_ReturnsBadRequest(string route) =>
        await _client.AssertBadRequestAsync($"{Url(route)}/pagedList?page=1&pagesize=0");

    [Theory]
    [MemberData(nameof(Paged))]
    public async Task PagedList_PastTheLastPage_ReturnsTheDocumentedContract(string route)
    {
        var contract = Contracts[route];

        await _client.AssertStatusAsync($"{Url(route)}/pagedList?page=999&pagesize=10", contract.PagedOutOfRange);
    }

    [Theory]
    [MemberData(nameof(Paged))]
    public async Task PagedList_RespectsPageSize(string route)
    {
        var envelope = await _client.GetJsonAsync<JsonElement>($"{Url(route)}/pagedList?page=1&pagesize=1");

        Assert.Equal(1, envelope.GetProperty("data").GetArrayLength());
    }
}
