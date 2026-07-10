using System.ComponentModel;
using Microsoft.EntityFrameworkCore;
using ModelContextProtocol.Server;

namespace api.Mcp;

/// <summary>
/// Generic, live-data tools that dispatch through the <see cref="ResourceCatalog"/> so a
/// single small set of tools covers every API Colombia resource. The <see cref="DBContext"/>
/// is injected per request from the MCP request scope (read-only, NoTracking).
/// </summary>
[McpServerToolType]
public class DataTools
{
    private const string ResourceParamDescription =
        "Resource key from list_colombia_resources (e.g. 'city', 'department', 'president', 'holiday').";

    [McpServerTool(Name = "get_country_info", ReadOnly = true, Idempotent = true),
     Description("Returns general information about Colombia (time zone, languages, currency, surface, population, etc.).")]
    public static object GetCountryInfo(DBContext db)
    {
        var country = db.Countries.FirstOrDefault();
        return country is null ? ToolError("No country information available.") : country;
    }

    [McpServerTool(Name = "list_items", ReadOnly = true, Idempotent = true),
     Description("Lists all items of a resource. Optionally sorted by 'sortBy' (a field of the resource) and 'sortDirection' ('asc' or 'desc').")]
    public static object ListItems(
        DBContext db,
        [Description(ResourceParamDescription)] string resource,
        [Description("Field to sort by (e.g. 'Id', 'name'). Optional.")] string? sortBy = null,
        [Description("'asc' or 'desc'. Optional.")] string? sortDirection = null)
    {
        if (!ResourceCatalog.TryGet(resource, out var descriptor))
            return UnknownResource(resource);

        var (data, validSort) = descriptor.List(db, sortBy, sortDirection);
        return validSort ? data! : InvalidSort();
    }

    [McpServerTool(Name = "get_item_by_id", ReadOnly = true, Idempotent = true),
     Description("Returns a single item of a resource by its numeric Id.")]
    public static object GetItemById(
        DBContext db,
        [Description(ResourceParamDescription)] string resource,
        [Description("Numeric Id of the item (must be greater than 0).")] int id)
    {
        if (!ResourceCatalog.TryGet(resource, out var descriptor))
            return UnknownResource(resource);

        if (id <= 0)
            return ToolError("Id must be greater than 0.");

        var item = descriptor.GetById(db, id);
        return item ?? ToolError($"No '{descriptor.Key}' found with Id {id}.");
    }

    [McpServerTool(Name = "get_items_by_name", ReadOnly = true, Idempotent = true),
     Description("Returns items of a resource whose Name contains the given text (case-insensitive). Only valid for resources that expose a Name field.")]
    public static object GetItemsByName(
        DBContext db,
        [Description(ResourceParamDescription)] string resource,
        [Description("Name (or part of it) to match.")] string name)
    {
        if (!ResourceCatalog.TryGet(resource, out var descriptor))
            return UnknownResource(resource);

        if (!descriptor.SupportsByName)
            return ToolError($"Resource '{descriptor.Key}' does not support lookup by name. Use search_items instead.");

        return descriptor.GetByName(db, name);
    }

    [McpServerTool(Name = "search_items", ReadOnly = true, Idempotent = true),
     Description("Searches a resource for a keyword across its text fields (accent-insensitive) and returns the matches.")]
    public static object SearchItems(
        DBContext db,
        [Description(ResourceParamDescription)] string resource,
        [Description("Keyword to search for.")] string keyword)
    {
        if (!ResourceCatalog.TryGet(resource, out var descriptor))
            return UnknownResource(resource);

        return descriptor.Search(db, keyword);
    }

    [McpServerTool(Name = "list_items_paged", ReadOnly = true, Idempotent = true),
     Description("Returns a page of items of a resource. Includes page, pageSize, totalRecords, pageCount and data. Supports optional sorting.")]
    public static object ListItemsPaged(
        DBContext db,
        [Description(ResourceParamDescription)] string resource,
        [Description("1-based page number.")] int page = 1,
        [Description("Number of items per page.")] int pageSize = 10,
        [Description("Field to sort by. Optional.")] string? sortBy = null,
        [Description("'asc' or 'desc'. Optional.")] string? sortDirection = null)
    {
        if (!ResourceCatalog.TryGet(resource, out var descriptor))
            return UnknownResource(resource);

        if (page <= 0 || pageSize <= 0)
            return ToolError("'page' and 'pageSize' must be greater than 0.");

        var (data, validSort) = descriptor.Paged(db, page, pageSize, sortBy, sortDirection);
        return validSort ? data! : InvalidSort();
    }

    private static object UnknownResource(string resource)
        => ToolError($"Unknown resource '{resource}'. Call list_colombia_resources for valid keys.");

    private static object InvalidSort()
        => ToolError("Invalid sort parameter. 'sortBy' must be a field of the resource and 'sortDirection' must be 'asc' or 'desc'.");

    private static object ToolError(string message) => new { error = message };
}
