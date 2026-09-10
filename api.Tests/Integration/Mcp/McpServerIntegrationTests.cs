using System.Text.Json;
using ModelContextProtocol.Client;
using ModelContextProtocol.Protocol;
using api.Tests.Infrastructure;

namespace api.Tests.Integration.Mcp;

/// <summary>
/// Deliberately on its own <see cref="IsolatedFactory"/> rather than the shared host. Each test
/// opens an MCP session over the Streamable HTTP transport, and running those sessions against a
/// host that the rest of the suite is also driving makes `tools/list` intermittently come back
/// without a reply.
/// </summary>
public class McpServerIntegrationTests : IClassFixture<IsolatedFactory>, IAsyncLifetime
{
    private readonly IsolatedFactory _factory;
    private McpClient _client = null!;

    public McpServerIntegrationTests(IsolatedFactory factory)
    {
        _factory = factory;
        _factory.EnsureSeeded();
    }

    public async Task InitializeAsync()
    {
        var httpClient = _factory.CreateClient();
        var transport = new HttpClientTransport(
            new HttpClientTransportOptions
            {
                Endpoint = new Uri(httpClient.BaseAddress!, "api/v1/mcp"),
                TransportMode = HttpTransportMode.StreamableHttp
            },
            httpClient);

        _client = await McpClient.CreateAsync(transport);
    }

    public async Task DisposeAsync() => await _client.DisposeAsync();

    [Fact]
    public async Task ListTools_AdvertisesTheMcpTools()
    {
        var tools = await _client.ListToolsAsync();
        var names = tools.Select(t => t.Name).ToList();

        Assert.Contains("list_colombia_resources", names);
        Assert.Contains("get_api_reference", names);
        Assert.Contains("get_country_info", names);
        Assert.Contains("list_items", names);
        Assert.Contains("get_item_by_id", names);
        Assert.Contains("search_items", names);
        Assert.Contains("list_items_paged", names);
    }

    [Fact]
    public async Task ReadResource_ReturnsCatalog()
    {
        var result = await _client.ReadResourceAsync("colombia://catalog");
        var text = string.Concat(result.Contents.OfType<TextResourceContents>().Select(c => c.Text));
        Assert.Contains("city", text);
        Assert.Contains("department", text);
    }

    [Fact]
    public async Task ReadResource_ReturnsSingleCatalogEntry()
    {
        var result = await _client.ReadResourceAsync("colombia://catalog/city");
        var text = string.Concat(result.Contents.OfType<TextResourceContents>().Select(c => c.Text));
        Assert.Contains("\"key\": \"city\"", text);
        Assert.Contains("list_items", text);
    }

    [Fact]
    public async Task ListColombiaResources_ReturnsTheCatalog()
    {
        var result = await _client.CallToolAsync("list_colombia_resources");

        Assert.NotEqual(true, result.IsError);
        var payload = GetPayload(result);
        Assert.Contains("city", payload);
        Assert.Contains("department", payload);
    }

    [Fact]
    public async Task GetItemById_ReturnsSeededCity()
    {
        var result = await _client.CallToolAsync(
            "get_item_by_id",
            new Dictionary<string, object?> { ["resource"] = "city", ["id"] = 1 });

        Assert.NotEqual(true, result.IsError);
        Assert.Contains("Medellín", GetPayload(result));
    }

    [Fact]
    public async Task GetCountryInfo_ReturnsColombia()
    {
        var result = await _client.CallToolAsync("get_country_info");

        Assert.NotEqual(true, result.IsError);
        Assert.Contains("Colombia", GetPayload(result));
    }

    [Fact]
    public async Task ListItems_ReturnsSeededPresidents()
    {
        var result = await _client.CallToolAsync(
            "list_items",
            new Dictionary<string, object?> { ["resource"] = "president" });

        Assert.NotEqual(true, result.IsError);
        Assert.Contains("Rafael", GetPayload(result));
    }

    [Fact]
    public async Task GetApiReference_DescribesTheRestRoutes()
    {
        var result = await _client.CallToolAsync("get_api_reference");

        Assert.NotEqual(true, result.IsError);
        Assert.Contains("api/v1", GetPayload(result));
    }

    [Fact]
    public async Task GetApiReference_CanBeScopedToOneResource()
    {
        var result = await _client.CallToolAsync(
            "get_api_reference",
            new Dictionary<string, object?> { ["resource"] = "volcano" });

        Assert.NotEqual(true, result.IsError);
        Assert.Contains("volcano", GetPayload(result), StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public async Task SearchItems_FindsASeededRow()
    {
        var result = await _client.CallToolAsync(
            "search_items",
            new Dictionary<string, object?> { ["resource"] = "president", ["keyword"] = "Rafael" });

        Assert.NotEqual(true, result.IsError);
        Assert.Contains("Rafael", GetPayload(result));
    }

    [Fact]
    public async Task SearchItems_WithoutMatches_ReturnsAnEmptyResult()
    {
        var result = await _client.CallToolAsync(
            "search_items",
            new Dictionary<string, object?> { ["resource"] = "president", ["keyword"] = "zzzzzzzzzzzz" });

        Assert.NotEqual(true, result.IsError);
        Assert.DoesNotContain("Rafael", GetPayload(result));
    }

    [Fact]
    public async Task ListItemsPaged_ReturnsOneRowPerPage()
    {
        var result = await _client.CallToolAsync(
            "list_items_paged",
            new Dictionary<string, object?> { ["resource"] = "president", ["page"] = 1, ["pageSize"] = 1 });

        Assert.NotEqual(true, result.IsError);
        Assert.Contains("Rafael", GetPayload(result));
    }

    [Fact]
    public async Task GetItemsByName_FindsASeededRow()
    {
        var result = await _client.CallToolAsync(
            "get_items_by_name",
            new Dictionary<string, object?> { ["resource"] = "city", ["name"] = "Medellín" });

        Assert.NotEqual(true, result.IsError);
        Assert.Contains("Medellín", GetPayload(result));
    }

    // ---------- Error paths ----------
    // The MCP tools report failures as an { error } payload rather than an HTTP status,
    // so an unknown resource key must come back described, not as a crash.

    [Fact]
    public async Task ListItems_WithUnknownResourceKey_ReportsAnError()
    {
        var result = await _client.CallToolAsync(
            "list_items",
            new Dictionary<string, object?> { ["resource"] = "notaresource" });

        Assert.Contains("error", GetPayload(result), StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public async Task GetItemById_WithUnknownId_ReportsAnError()
    {
        var result = await _client.CallToolAsync(
            "get_item_by_id",
            new Dictionary<string, object?> { ["resource"] = "city", ["id"] = 9999 });

        Assert.Contains("error", GetPayload(result), StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public async Task ReadResource_WithUnknownCatalogKey_DoesNotReturnAnEntry()
    {
        var result = await _client.ReadResourceAsync("colombia://catalog/notaresource");
        var text = string.Concat(result.Contents.OfType<TextResourceContents>().Select(c => c.Text));

        Assert.DoesNotContain("\"key\": \"notaresource\"", text);
    }

    // ---------- RelationalTools ----------
    // These mirror the Department sub-resource REST routes and had no coverage at all.

    [Fact]
    public async Task GetCitiesByDepartment_ReturnsTheDepartmentsCities()
    {
        var result = await _client.CallToolAsync(
            "get_cities_by_department",
            new Dictionary<string, object?> { ["departmentId"] = 1 });

        Assert.NotEqual(true, result.IsError);
        Assert.Contains("Medellín", GetPayload(result));
    }

    [Fact]
    public async Task GetNaturalAreasByDepartment_ReturnsTheDepartmentsAreas()
    {
        var result = await _client.CallToolAsync(
            "get_natural_areas_by_department",
            new Dictionary<string, object?> { ["departmentId"] = 1 });

        Assert.NotEqual(true, result.IsError);
        Assert.Contains("Parque Arví", GetPayload(result));
    }

    [Fact]
    public async Task GetTouristicAttractionsByDepartment_ReturnsTheDepartmentsAttractions()
    {
        var result = await _client.CallToolAsync(
            "get_touristic_attractions_by_department",
            new Dictionary<string, object?> { ["departmentId"] = 1 });

        Assert.NotEqual(true, result.IsError);
        Assert.Contains("Parque Explora", GetPayload(result));
    }

    [Theory]
    [InlineData("get_cities_by_department")]
    [InlineData("get_natural_areas_by_department")]
    [InlineData("get_touristic_attractions_by_department")]
    public async Task RelationalTool_WithNonPositiveDepartmentId_ReportsAnError(string tool)
    {
        var result = await _client.CallToolAsync(tool, new Dictionary<string, object?> { ["departmentId"] = 0 });

        Assert.Contains("departmentId must be greater than 0", GetPayload(result));
    }

    [Theory]
    [InlineData("get_cities_by_department")]
    [InlineData("get_natural_areas_by_department")]
    [InlineData("get_touristic_attractions_by_department")]
    public async Task RelationalTool_ForADepartmentWithNoRows_ReturnsAnEmptyList(string tool)
    {
        var result = await _client.CallToolAsync(tool, new Dictionary<string, object?> { ["departmentId"] = 9999 });

        Assert.NotEqual(true, result.IsError);
        Assert.DoesNotContain("Medellín", GetPayload(result));
    }

    [Fact]
    public async Task ListTools_AdvertisesTheRelationalTools()
    {
        var names = (await _client.ListToolsAsync()).Select(t => t.Name).ToList();

        Assert.Contains("get_cities_by_department", names);
        Assert.Contains("get_natural_areas_by_department", names);
        Assert.Contains("get_touristic_attractions_by_department", names);
    }

    // ---------- DataTools guards ----------

    [Theory]
    [InlineData("get_item_by_id")]
    [InlineData("get_items_by_name")]
    [InlineData("search_items")]
    [InlineData("list_items_paged")]
    public async Task DataTool_WithUnknownResourceKey_ReportsAnError(string tool)
    {
        var arguments = new Dictionary<string, object?> { ["resource"] = "notaresource" };

        // Fill in whatever else each tool requires; the resource check runs first.
        if (tool == "get_item_by_id") arguments["id"] = 1;
        if (tool == "get_items_by_name") arguments["name"] = "anything";
        if (tool == "search_items") arguments["keyword"] = "anything";

        var result = await _client.CallToolAsync(tool, arguments);

        Assert.Contains("Unknown resource", GetPayload(result));
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public async Task GetItemById_WithNonPositiveId_ReportsAnError(int id)
    {
        var result = await _client.CallToolAsync(
            "get_item_by_id",
            new Dictionary<string, object?> { ["resource"] = "city", ["id"] = id });

        Assert.Contains("Id must be greater than 0", GetPayload(result));
    }

    /// <summary>
    /// ConstitutionArticle and PostalCode have no Name property, so the catalog marks them as not
    /// supporting lookup by name and the tool points the caller at search_items instead.
    /// </summary>
    [Theory]
    [InlineData("constitutionarticle")]
    [InlineData("postalcode")]
    public async Task GetItemsByName_OnAResourceWithoutAName_PointsAtSearchInstead(string resource)
    {
        var result = await _client.CallToolAsync(
            "get_items_by_name",
            new Dictionary<string, object?> { ["resource"] = resource, ["name"] = "anything" });

        var payload = GetPayload(result);
        Assert.Contains("does not support lookup by name", payload);
        Assert.Contains("search_items", payload);
    }

    [Theory]
    [InlineData(0, 10)]
    [InlineData(-1, 10)]
    [InlineData(1, 0)]
    [InlineData(1, -5)]
    public async Task ListItemsPaged_WithANonPositivePageOrSize_ReportsAnError(int page, int pageSize)
    {
        var result = await _client.CallToolAsync(
            "list_items_paged",
            new Dictionary<string, object?> { ["resource"] = "president", ["page"] = page, ["pageSize"] = pageSize });

        Assert.Contains("must be greater than 0", GetPayload(result));
    }

    [Fact]
    public async Task ListItemsPaged_ReturnsTheFullEnvelope()
    {
        var result = await _client.CallToolAsync(
            "list_items_paged",
            new Dictionary<string, object?> { ["resource"] = "president", ["page"] = 1, ["pageSize"] = 2 });

        var payload = GetPayload(result);
        Assert.Contains("totalRecords", payload, StringComparison.OrdinalIgnoreCase);
        Assert.Contains("pageCount", payload, StringComparison.OrdinalIgnoreCase);
    }

    // ---------- Sorting ----------

    [Fact]
    public async Task ListItems_HonoursSorting()
    {
        var ascending = await _client.CallToolAsync(
            "list_items",
            new Dictionary<string, object?> { ["resource"] = "president", ["sortBy"] = "Name", ["sortDirection"] = "asc" });
        var descending = await _client.CallToolAsync(
            "list_items",
            new Dictionary<string, object?> { ["resource"] = "president", ["sortBy"] = "Name", ["sortDirection"] = "desc" });

        Assert.NotEqual(GetPayload(ascending), GetPayload(descending));
    }

    [Theory]
    [InlineData("list_items")]
    [InlineData("list_items_paged")]
    public async Task ListingTool_WithAnUnknownSortField_ReportsAnError(string tool)
    {
        var result = await _client.CallToolAsync(
            tool,
            new Dictionary<string, object?> { ["resource"] = "president", ["sortBy"] = "notAProperty", ["sortDirection"] = "asc" });

        Assert.Contains("Invalid sort parameter", GetPayload(result));
    }

    [Theory]
    [InlineData("list_items")]
    [InlineData("list_items_paged")]
    public async Task ListingTool_WithAnUnknownSortDirection_ReportsAnError(string tool)
    {
        var result = await _client.CallToolAsync(
            tool,
            new Dictionary<string, object?> { ["resource"] = "president", ["sortBy"] = "Name", ["sortDirection"] = "sideways" });

        Assert.Contains("Invalid sort parameter", GetPayload(result));
    }

    // ---------- GuidanceTools ----------

    [Fact]
    public async Task GetApiReference_WithUnknownResourceKey_ReportsAnErrorButStillExplainsTheConventions()
    {
        var result = await _client.CallToolAsync(
            "get_api_reference",
            new Dictionary<string, object?> { ["resource"] = "notaresource" });

        var payload = GetPayload(result);
        Assert.Contains("Unknown resource", payload);
        Assert.Contains("conventions", payload, StringComparison.OrdinalIgnoreCase);
    }

    private static string GetPayload(CallToolResult result)
    {
        var text = string.Concat(result.Content.OfType<TextContentBlock>().Select(b => b.Text));
        if (!string.IsNullOrEmpty(text))
        {
            return text;
        }

        return result.StructuredContent is JsonElement element ? element.GetRawText() : string.Empty;
    }
}
