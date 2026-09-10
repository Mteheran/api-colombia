using System.Text.Json;
using api.Const;
using api.Tests.Infrastructure;

namespace api.Tests.Integration.Platform;

/// <summary>
/// Keeps the published OpenAPI document in step with the API.
///
/// `docs/public/openapi.json` is checked in and feeds the VitePress reference at
/// docs.api-colombia.com, but nothing regenerated it: it sat at 1.0.5 while the API had moved to
/// 1.7.x, so the public reference documented endpoints and response types that no longer matched.
///
/// This test compares the checked-in document against the one the running app produces. To adopt
/// a legitimate change, regenerate it:
///
///     UPDATE_OPENAPI=1 dotnet test --filter OpenApiDocumentTests
/// </summary>
[Collection(SharedApiCollection.Name)]
public class OpenApiDocumentTests(ApiColombiaFactory factory) : IntegrationTestBase(factory)
{
    private const string UpdateFlag = "UPDATE_OPENAPI";

    private static readonly JsonSerializerOptions Indented = new() { WriteIndented = true };

    private async Task<string> GenerateAsync()
    {
        var response = await _client.GetAsync("/swagger/v1/swagger.json");
        response.EnsureSuccessStatusCode();

        // Round-trip through the parser so formatting can never be the reason this test fails.
        using var document = JsonDocument.Parse(await response.Content.ReadAsStringAsync());
        return JsonSerializer.Serialize(document.RootElement, Indented) + Environment.NewLine;
    }

    private static string ReadCheckedIn()
    {
        using var document = JsonDocument.Parse(File.ReadAllText(RepositoryPaths.OpenApiDocument));
        return JsonSerializer.Serialize(document.RootElement, Indented) + Environment.NewLine;
    }

    [Fact]
    public async Task CheckedInDocument_MatchesTheOneTheApiProduces()
    {
        var generated = await GenerateAsync();

        if (Environment.GetEnvironmentVariable(UpdateFlag) == "1")
        {
            await File.WriteAllTextAsync(RepositoryPaths.OpenApiDocument, generated);
            return;
        }

        Assert.True(
            ReadCheckedIn() == generated,
            $"docs/public/openapi.json is out of date. Regenerate it with:\n"
            + $"    {UpdateFlag}=1 dotnet test --filter OpenApiDocumentTests");
    }

    [Fact]
    public async Task GeneratedDocument_CarriesTheCurrentVersion()
    {
        using var document = JsonDocument.Parse(await GenerateAsync());

        Assert.Equal(
            VersionInfo.CurrentVersion,
            document.RootElement.GetProperty("info").GetProperty("version").GetString());
    }

    [Fact]
    public void CheckedInDocument_CarriesTheCurrentVersion()
    {
        using var document = JsonDocument.Parse(File.ReadAllText(RepositoryPaths.OpenApiDocument));

        Assert.Equal(
            VersionInfo.CurrentVersion,
            document.RootElement.GetProperty("info").GetProperty("version").GetString());
    }
}
