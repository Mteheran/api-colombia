using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.Json.Serialization.Metadata;
using api.Utils;

namespace api.Mcp;

/// <summary>
/// Registers and maps the API Colombia MCP (Model Context Protocol) server so AI agents
/// can consume the same public data the REST API exposes. The server is hosted inside the
/// existing web application and is purely additive: it does not alter any existing route.
/// </summary>
public static class McpServerExtensions
{
    /// <summary>Route the Streamable HTTP MCP endpoint is served from.</summary>
    public const string McpRoute = $"/{Util.API_ROUTE}{Util.API_VERSION}mcp"; // /api/v1/mcp

    /// <summary>
    /// Serializer options for tool return values. Mirrors the HTTP pipeline configuration
    /// in Program.cs so navigation-property cycles (e.g. City -> Department -> City) and
    /// DateOnly values are handled identically to the REST responses.
    /// </summary>
    private static readonly JsonSerializerOptions ToolSerializerOptions = CreateToolSerializerOptions();

    public static WebApplicationBuilder AddApiColombiaMcp(this WebApplicationBuilder builder)
    {
        builder.Services
            .AddMcpServer(options =>
            {
                options.ServerInfo = new() { Name = "api-colombia", Version = api.Const.VersionInfo.CurrentVersion };
                options.ServerInstructions =
                    "Provides read-only public information about Colombia (geography, government, culture, nature). " +
                    "Call 'list_colombia_resources' to discover the available resources and operations, and " +
                    "'get_api_reference' for usage conventions (sorting, pagination, keyword search). " +
                    "Use the generic data tools (list_items, get_item_by_id, get_items_by_name, search_items, " +
                    "list_items_paged) with a 'resource' key from the catalog to fetch live data.";
            })
            // Stateless mode: POST-only (no SSE GET), so the global output-cache base policy
            // never touches MCP and the endpoint scales horizontally without session affinity.
            .WithHttpTransport(options => options.Stateless = true)
            .WithTools<GuidanceTools>(ToolSerializerOptions)
            .WithTools<DataTools>(ToolSerializerOptions)
            .WithTools<RelationalTools>(ToolSerializerOptions)
            .WithResources<CatalogResources>();

        return builder;
    }

    public static WebApplication MapApiColombiaMcp(this WebApplication app)
    {
        app.MapMcp(McpRoute);
        return app;
    }

    private static JsonSerializerOptions CreateToolSerializerOptions()
    {
        var options = new JsonSerializerOptions(JsonSerializerDefaults.Web)
        {
            // AIFunctionFactory (used internally by the MCP SDK to build tool schemas) calls
            // JsonSerializerOptions.MakeReadOnly(), which throws unless a TypeInfoResolver is
            // set. JsonSerializerDefaults.Web does not set one on .NET 10, so we attach the
            // reflection-based default resolver explicitly.
            TypeInfoResolver = new DefaultJsonTypeInfoResolver(),
            ReferenceHandler = ReferenceHandler.IgnoreCycles,
            // Emit accented characters (e.g. "Medellín") as readable UTF-8 rather than
            // \uXXXX escapes, so the text an agent receives matches the source data.
            Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping
        };
        options.Converters.Add(new DateOnlyJsonConverter());
        return options;
    }
}
