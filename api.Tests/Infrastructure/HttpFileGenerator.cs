using System.Text;
using System.Text.Json;

namespace api.Tests.Infrastructure;

/// <summary>
/// Builds <c>api/api.http</c> — the request file Visual Studio (and VS Code REST Client / JetBrains
/// HTTP Client) runs — from the OpenAPI document the app produces.
///
/// Writing the file by hand would drift the same way <c>docs/public/openapi.json</c> did, so the
/// endpoint list comes from Swagger and only the example values are curated here: realistic names,
/// codes and ids that return data against the live API, so every request is useful as-is.
///
/// The host is not defined in the file: it comes from <c>api/http-client.env.json</c>, because a
/// variable declared in a .http file overrides the environment file and would pin every request to
/// one host.
/// </summary>
public static class HttpFileGenerator
{
    /// <summary>Paths that are not part of the public, consumable API.</summary>
    private static readonly HashSet<string> ExcludedPaths = ["/db-creation"];

    /// <summary>Fallback example for a parameter, by parameter name (case-insensitive).</summary>
    private static readonly Dictionary<string, string> DefaultValues = new(StringComparer.OrdinalIgnoreCase)
    {
        ["id"] = "1",
        ["cityId"] = "1",
        ["year"] = "2026",
        ["month"] = "12",
        ["chapternumber"] = "1",
        ["includeSunday"] = "false",
        ["Page"] = "{{page}}",
        ["PageSize"] = "{{pageSize}}",
        ["SortBy"] = "Id",
        ["SortDirection"] = "asc",
    };

    /// <summary>
    /// Per-resource examples (keyed by Swagger tag, then parameter name). Several /name endpoints
    /// match the whole name exactly, so those examples are full names taken from the live data.
    /// </summary>
    private static readonly Dictionary<string, Dictionary<string, string>> TagValues = new()
    {
        ["Airport"] = new() { ["name"] = "El Dorado", ["keyword"] = "Internacional" },
        ["City"] = new() { ["name"] = "Medellín", ["keyword"] = "Cartagena" },
        ["ConstitutionArticle"] = new() { ["keyword"] = "libertad" },
        ["Department"] = new() { ["id"] = "5", ["name"] = "Antioquia", ["keyword"] = "Santander" },
        ["HeritageCity"] = new() { ["name"] = "Barichara", ["keyword"] = "Boyacá" },
        ["HigherEducationInstitution"] = new() { ["name"] = "Universidad Nacional", ["keyword"] = "Medellín" },
        ["IndigenousReservation"] = new() { ["name"] = "Pared Y Parecito", ["keyword"] = "Resolución" },
        ["IntangibleHeritage"] = new() { ["name"] = "Músicas de marimba y cantos tradicionales del Pacífico sur de Colombia", ["keyword"] = "Wayuu" },
        ["InvasiveSpecie"] = new() { ["name"] = "Acacia", ["keyword"] = "Retamo" },
        ["NativeCommunity"] = new() { ["name"] = "Achagua Piapoco", ["keyword"] = "Arhuaco" },
        ["NaturalArea"] = new() { ["name"] = "Tayrona", ["keyword"] = "Cueva" },
        ["PostalCode"] = new() { ["code"] = "687519", ["keyword"] = "Rural", ["cityId"] = "972" },
        ["President"] = new() { ["name"] = "Rafael", ["keyword"] = "Liberal", ["year"] = "1990" },
        ["Radio"] = new() { ["name"] = "Fantastica", ["keyword"] = "FM" },
        ["TelevisionChannel"] = new() { ["name"] = "Teleantioquia", ["keyword"] = "Canal" },
        ["TouristicAttraction"] = new() { ["name"] = "Museo del oro", ["keyword"] = "Museo" },
        ["TraditionalFairAndFestival"] = new() { ["name"] = "Fiestas del 20 de enero", ["keyword"] = "Carnaval" },
        ["TypicalDish"] = new() { ["name"] = "Bandeja Paisa", ["keyword"] = "Ajiaco" },
        ["UrbanCenter"] = new() { ["code"] = "05001000", ["keyword"] = "CM", ["cityId"] = "12" },
        ["Volcano"] = new() { ["name"] = "Galeras", ["keyword"] = "Nariño" },
    };

    /// <summary>Overrides for a single path, where the resource default would return nothing.</summary>
    private static readonly Dictionary<string, Dictionary<string, string>> PathValues = new()
    {
        ["/api/v1/City/{id}/volcanoes"] = new() { ["id"] = "1042" },
        ["/api/v1/City/{id}/televisionchannels"] = new() { ["id"] = "167" },
        // {id} here is the department, not the heritage item.
        ["/api/v1/IntangibleHeritage/{id}/department"] = new() { ["id"] = "6" },
        ["/api/v1/Department/{id}/volcanoes"] = new() { ["id"] = "30" },
    };

    /// <summary>The request line of every request in the file, in order (host variable included).</summary>
    public static IEnumerable<string> RequestLines(string httpFile) =>
        httpFile.Split('\n')
            .Select(line => line.TrimEnd('\r'))
            .Where(line => line.StartsWith("GET ", StringComparison.Ordinal));

    public static string Generate(string openApiJson)
    {
        using var document = JsonDocument.Parse(openApiJson);
        var root = document.RootElement;
        var version = root.GetProperty("info").GetProperty("version").GetString();

        var sb = new StringBuilder();
        sb.AppendLine("# API Colombia — every public endpoint, ready to run.");
        sb.AppendLine("#");
        sb.AppendLine("# GENERATED from the OpenAPI document — do not edit by hand. Regenerate from api.Tests/ with:");
        sb.AppendLine("#     UPDATE_HTTP_FILE=1 dotnet test --filter HttpFileTests");
        sb.AppendLine("#");
        sb.AppendLine("# Pick an environment (local, local-http, production) in the editor's environment selector;");
        sb.AppendLine("# hosts live in http-client.env.json. Personal overrides go in http-client.env.json.user.");
        sb.AppendLine("#");
        sb.AppendLine("# Common options on list endpoints: ?sortBy=<property>&sortDirection=asc|desc");
        sb.AppendLine($"# API version: {version}");
        sb.AppendLine();
        sb.AppendLine("@page = 1");
        sb.AppendLine("@pageSize = 10");

        string? currentTag = null;

        foreach (var path in root.GetProperty("paths").EnumerateObject())
        {
            if (ExcludedPaths.Contains(path.Name))
            {
                continue;
            }

            foreach (var operation in path.Value.EnumerateObject())
            {
                if (!operation.Name.Equals("get", StringComparison.OrdinalIgnoreCase))
                {
                    continue;
                }

                var op = operation.Value;
                var tag = op.TryGetProperty("tags", out var tags) && tags.GetArrayLength() > 0
                    ? tags[0].GetString()!
                    : "Other";

                if (tag != currentTag)
                {
                    currentTag = tag;
                    sb.AppendLine();
                    sb.AppendLine($"# ═════════════════════════════ {tag} ═════════════════════════════");
                }

                var summary = op.TryGetProperty("summary", out var s) ? s.GetString()?.Trim() : null;

                sb.AppendLine();
                sb.AppendLine($"### {summary ?? path.Name}");
                sb.AppendLine($"GET {{{{host}}}}{BuildUrl(path.Name, tag, op)}");
                sb.AppendLine("Accept: application/json");
            }
        }

        return sb.ToString();
    }

    private static string BuildUrl(string pathTemplate, string tag, JsonElement op)
    {
        var url = pathTemplate;
        var query = new List<string>();

        if (op.TryGetProperty("parameters", out var parameters))
        {
            // Required parameters (page, pageSize) read first; optional ones follow.
            foreach (var parameter in parameters.EnumerateArray()
                         .OrderByDescending(p => p.TryGetProperty("required", out var req) && req.GetBoolean()))
            {
                var name = parameter.GetProperty("name").GetString()!;
                var location = parameter.GetProperty("in").GetString();
                var required = parameter.TryGetProperty("required", out var r) && r.GetBoolean();

                if (location == "path")
                {
                    url = url.Replace($"{{{name}}}", EscapeValue(ExampleFor(pathTemplate, tag, name)));
                }
                // Optional list sorting is noise on every request; the header documents it once.
                // pagedList shows it because it is part of that endpoint's contract.
                else if (location == "query" && (required || !IsOptionalSort(name) || IsPagedList(pathTemplate)))
                {
                    query.Add($"{name}={EscapeValue(ExampleFor(pathTemplate, tag, name))}");
                }
            }
        }

        return query.Count == 0 ? url : $"{url}?{string.Join('&', query)}";
    }

    private static bool IsOptionalSort(string name) =>
        name.Equals("sortBy", StringComparison.OrdinalIgnoreCase)
        || name.Equals("sortDirection", StringComparison.OrdinalIgnoreCase);

    private static bool IsPagedList(string pathTemplate) =>
        pathTemplate.EndsWith("/pagedList", StringComparison.OrdinalIgnoreCase);

    /// <summary>
    /// Escapes only spaces: accented letters stay readable (the HTTP clients encode them on send) and
    /// <c>{{variable}}</c> references stay intact so the editor can substitute them.
    /// </summary>
    private static string EscapeValue(string value) => value.Replace(" ", "%20");

    private static string ExampleFor(string pathTemplate, string tag, string parameter)
    {
        if (PathValues.TryGetValue(pathTemplate, out var byPath) && byPath.TryGetValue(parameter, out var v1))
        {
            return v1;
        }

        if (TagValues.TryGetValue(tag, out var byTag) && byTag.TryGetValue(parameter, out var v2))
        {
            return v2;
        }

        if (DefaultValues.TryGetValue(parameter, out var v3))
        {
            return v3;
        }

        throw new InvalidOperationException(
            $"No example value for parameter '{parameter}' of {pathTemplate}. "
            + $"Add one to {nameof(HttpFileGenerator)}.");
    }
}
