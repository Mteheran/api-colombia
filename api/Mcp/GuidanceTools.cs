using System.ComponentModel;
using ModelContextProtocol.Server;

namespace api.Mcp;

/// <summary>
/// "How to use the API" tools. These teach an agent which resources exist and how to
/// call them, so it can then use the generic data tools in <see cref="DataTools"/>.
/// </summary>
[McpServerToolType]
public class GuidanceTools
{
    [McpServerTool(Name = "list_colombia_resources", ReadOnly = true, Idempotent = true),
     Description("Lists every resource the API Colombia MCP server exposes, with its key, description, REST route, and the operations it supports. Call this first to discover valid 'resource' values for the data tools.")]
    public static object ListColombiaResources()
        => ResourceCatalog.All
            .Select(r => new
            {
                resource = r.Key,
                description = r.Description,
                route = r.Route,
                operations = r.Operations
            })
            .OrderBy(r => r.resource)
            .ToList();

    [McpServerTool(Name = "get_api_reference", ReadOnly = true, Idempotent = true),
     Description("Returns usage guidance for the API Colombia data tools: shared conventions (sorting, pagination, keyword search) and per-resource details. Pass a 'resource' key to scope it to one resource, or omit it to get the full reference.")]
    public static object GetApiReference(
        [Description("Optional resource key (e.g. 'city', 'department'). Omit for all resources.")] string? resource = null)
    {
        var conventions = new
        {
            baseUrl = "https://api-colombia.com",
            authentication = "None. All data is public and read-only (GET only).",
            sorting = "list_items and list_items_paged accept 'sortBy' (any field of the resource, e.g. Id, name) and 'sortDirection' ('asc' or 'desc').",
            pagination = "list_items_paged accepts 'page' (1-based) and 'pageSize'; it returns page, pageSize, totalRecords, pageCount and data.",
            search = "search_items matches a keyword against the resource's text fields (accent-insensitive). get_items_by_name matches the Name field only."
        };

        if (!string.IsNullOrWhiteSpace(resource))
        {
            if (!ResourceCatalog.TryGet(resource, out var descriptor))
            {
                return new { error = $"Unknown resource '{resource}'. Call list_colombia_resources for valid keys.", conventions };
            }

            return new
            {
                conventions,
                resource = new
                {
                    key = descriptor.Key,
                    description = descriptor.Description,
                    route = descriptor.Route,
                    operations = descriptor.Operations
                }
            };
        }

        return new
        {
            conventions,
            resources = ResourceCatalog.All
                .Select(r => new { key = r.Key, description = r.Description, route = r.Route, operations = r.Operations })
                .OrderBy(r => r.key)
                .ToList()
        };
    }
}
