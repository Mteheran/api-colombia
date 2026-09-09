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
