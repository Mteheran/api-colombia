using System.Net;
using System.Text.Json;
using api.Tests.Infrastructure;

namespace api.Tests.Integration.Platform;

/// <summary>
/// Keeps <c>api/api.http</c> — the request file Visual Studio runs — in step with the API.
///
/// The file is generated from the OpenAPI document by <see cref="HttpFileGenerator"/>. To adopt a
/// new or changed endpoint, regenerate it:
///
///     UPDATE_HTTP_FILE=1 dotnet test --filter HttpFileTests
/// </summary>
[Collection(SharedApiCollection.Name)]
public class HttpFileTests(ApiColombiaFactory factory) : IntegrationTestBase(factory)
{
    private const string UpdateFlag = "UPDATE_HTTP_FILE";

    private async Task<string> GenerateAsync()
    {
        var response = await _client.GetAsync("/swagger/v1/swagger.json");
        response.EnsureSuccessStatusCode();
        return HttpFileGenerator.Generate(await response.Content.ReadAsStringAsync());
    }

    private static string ReadCheckedIn() =>
        File.ReadAllText(RepositoryPaths.HttpFile).ReplaceLineEndings("\n");

    [Fact]
    public async Task CheckedInFile_MatchesTheOneGeneratedFromTheApi()
    {
        var generated = (await GenerateAsync()).ReplaceLineEndings("\n");

        if (Environment.GetEnvironmentVariable(UpdateFlag) == "1")
        {
            await File.WriteAllTextAsync(RepositoryPaths.HttpFile, generated);
            return;
        }

        Assert.True(
            ReadCheckedIn() == generated,
            $"api/api.http is out of date. Regenerate it with:\n"
            + $"    {UpdateFlag}=1 dotnet test --filter HttpFileTests");
    }

    [Fact]
    public void EveryRequest_IsAnswered_WithoutAServerError()
    {
        var requests = HttpFileGenerator.RequestLines(ReadCheckedIn()).ToList();
        Assert.NotEmpty(requests);

        var failures = new List<string>();

        foreach (var line in requests)
        {
            var url = line["GET ".Length..]
                .Replace("{{host}}", string.Empty)
                .Replace("{{page}}", "1")
                .Replace("{{pageSize}}", "10");

            Assert.DoesNotContain("{{", url);

            var status = _client.GetAsync(url).GetAwaiter().GetResult().StatusCode;

            // The seed data is small, so a curated example may legitimately be 404 here; what must
            // never happen is a 5xx, a 400 from a malformed example, or a route that does not match.
            if ((int)status >= 500 || status is HttpStatusCode.BadRequest or HttpStatusCode.MethodNotAllowed)
            {
                failures.Add($"{(int)status} {url}");
            }
        }

        Assert.True(failures.Count == 0, "Requests in api.http failed:\n" + string.Join('\n', failures));
    }

    [Fact]
    public void EnvironmentFile_DefinesTheHostForEveryEnvironment()
    {
        using var document = JsonDocument.Parse(File.ReadAllText(RepositoryPaths.HttpEnvironmentFile));

        var environments = document.RootElement.EnumerateObject()
            .Where(e => e.Name != "$shared")
            .ToList();

        Assert.NotEmpty(environments);
        Assert.All(environments, e =>
            Assert.StartsWith("http", e.Value.GetProperty("host").GetString()));
    }
}
