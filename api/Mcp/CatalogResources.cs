using System.ComponentModel;
using System.Text.Encodings.Web;
using System.Text.Json;
using ModelContextProtocol.Server;

namespace api.Mcp;

/// <summary>
/// MCP resources that expose the catalog of tables (entities) available to the generic
/// data tools. Clients like MCP Inspector / Claude Desktop can browse these resources
/// directly, so users get a discoverable list of every table they can query through the
/// tools (list_items, get_item_by_id, get_items_by_name, search_items, list_items_paged).
///
/// URI scheme:
///   colombia://catalog             -> full index of tables
///   colombia://catalog/{resource}  -> details of a single table (key, description, route, operations)
/// </summary>
[McpServerResourceType]
public class CatalogResources
{
    // Resource functions must return a supported type (string, ResourceContents, ...),
    // not an arbitrary object like the tools do. We serialize the payload to a JSON string
    // ourselves; the SDK wraps it as text content with the declared application/json MIME type.
    private static readonly JsonSerializerOptions SerializerOptions = new(JsonSerializerDefaults.Web)
    {
        WriteIndented = true,
        Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping
    };

    private static string ToJson(object payload) => JsonSerializer.Serialize(payload, SerializerOptions);

    [McpServerResource(
        Name = "colombia_catalog",
        UriTemplate = "colombia://catalog",
        MimeType = "application/json"),
     Description(
         "Full catalog of tables (entities) exposed by the API Colombia MCP server. " +
         "Each entry contains the resource key to pass to the data tools (list_items, " +
         "get_item_by_id, get_items_by_name, search_items, list_items_paged), its human " +
         "description, the equivalent REST route, and the operations it supports.")]
    public static string GetCatalog()
        => ToJson(new
        {
            count = ResourceCatalog.All.Count(),
            resources = ResourceCatalog.All
                .Select(r => new
                {
                    key = r.Key,
                    description = r.Description,
                    route = r.Route,
                    operations = r.Operations,
                    uri = $"colombia://catalog/{r.Key}"
                })
                .OrderBy(r => r.key)
                .ToList()
        });

    [McpServerResource(
        Name = "colombia_catalog_entry",
        UriTemplate = "colombia://catalog/{resource}",
        MimeType = "application/json"),
     Description(
         "Details for a single API Colombia table. Use the returned 'key' with the data " +
         "tools to query the underlying data.")]
    public static string GetCatalogEntry(
        [Description("Resource key (e.g. 'city', 'department', 'president', 'holiday').")]
        string resource)
    {
        if (!ResourceCatalog.TryGet(resource, out var descriptor))
        {
            return ToJson(new
            {
                error = $"Unknown resource '{resource}'. See colombia://catalog for the full list."
            });
        }

        return ToJson(new
        {
            key = descriptor.Key,
            description = descriptor.Description,
            route = descriptor.Route,
            operations = descriptor.Operations,
            usage = new
            {
                list = new { tool = "list_items", arguments = new { resource = descriptor.Key } },
                byId = new { tool = "get_item_by_id", arguments = new { resource = descriptor.Key, id = 1 } },
                byName = descriptor.SupportsByName
                    ? new { tool = "get_items_by_name", arguments = new { resource = descriptor.Key, name = "..." } }
                    : null,
                search = new { tool = "search_items", arguments = new { resource = descriptor.Key, keyword = "..." } },
                paged = new { tool = "list_items_paged", arguments = new { resource = descriptor.Key, page = 1, pageSize = 10 } }
            }
        });
    }
}