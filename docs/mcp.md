# MCP Server (Model Context Protocol)

API Colombia ships with a built-in **Model Context Protocol** server so AI agents can consume the same public data available through the REST API — without learning the REST surface by hand.

## Endpoint

- **URL:** `https://api-colombia.com/api/v1/mcp`
- **Transport:** Streamable HTTP (JSON-RPC over `POST`)
- **Auth:** None. All data is public and read-only.
- **State:** Stateless — every request is independent and safe to scale horizontally.

## Client configuration

### Claude Desktop / Claude Code

Add an entry under `mcpServers` in your MCP client config:

```json
{
  "mcpServers": {
    "api-colombia": {
      "type": "http",
      "url": "https://api-colombia.com/api/v1/mcp"
    }
  }
}
```

### Built-in browser inspector

The API ships with a self-contained web tester — no install required. Open it at:

- **Local:** `https://localhost:7274/mcp` (or `http://localhost:5204/mcp`)
- **Production:** [`https://api-colombia.com/mcp`](https://api-colombia.com/mcp)

It auto-connects to the MCP endpoint on the same origin and lets you list and call every tool (with forms generated from each tool's input schema), browse resources and resource templates, and send raw JSON-RPC requests.

### MCP Inspector (npm)

For the official tool:

```bash
npx @modelcontextprotocol/inspector
```

Then connect to `http://localhost:5204/api/v1/mcp` when running the API locally.

## Available tools

### Discovery / guidance

| Tool | Description |
| --- | --- |
| `list_colombia_resources` | Lists every resource the MCP server exposes (key, description, REST route, supported operations). Call this first. |
| `get_api_reference` | Returns usage conventions (sorting, pagination, search) and per-resource details. Pass a `resource` key or omit for all. |

### Generic data tools

All of these take a `resource` key from `list_colombia_resources` (e.g. `city`, `department`, `president`, `holiday`).

| Tool | Description |
| --- | --- |
| `get_country_info` | General information about Colombia. |
| `list_items` | Lists all items of a resource. Optional `sortBy`, `sortDirection`. |
| `get_item_by_id` | Single item by numeric `Id`. |
| `get_items_by_name` | Items whose `Name` contains the given text (case-insensitive). |
| `search_items` | Keyword search across text fields (accent-insensitive). |
| `list_items_paged` | Paginated list with `page`, `pageSize`, `totalRecords`, `pageCount`, `data`. |

### Relational tools

| Tool | Description |
| --- | --- |
| `get_cities_by_department` | Cities belonging to a department. |
| `get_natural_areas_by_department` | Natural areas located in a department. |
| `get_touristic_attractions_by_department` | Tourist attractions in a department. |

## Resources

The server also exposes browsable MCP **resources** so clients (MCP Inspector, Claude Desktop) can discover the catalog of tables directly.

| URI | Description |
| --- | --- |
| `colombia://catalog` | Full index of every table, with its resource key, description, REST route, and supported operations. |
| `colombia://catalog/{resource}` | Details for a single table, including ready-to-use example tool calls. |

## Example calls

```json
{ "name": "get_item_by_id", "arguments": { "resource": "city", "id": 1 } }
```

```json
{ "name": "list_items_paged", "arguments": { "resource": "president", "page": 1, "pageSize": 10, "sortBy": "Id", "sortDirection": "desc" } }
```

## Notes

- The MCP server is purely additive; it does not change, remove, or alter any existing REST route, model, or migration.
- All data is read-only (`GET`-only). There are no write operations.
- Responses reuse the same sorting, pagination, and keyword-search semantics as the REST API, so results match what the equivalent endpoint returns.