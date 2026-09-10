namespace api.Tests.Infrastructure;

/// <summary>
/// Locates files that live in the repository rather than next to the test assembly, so tests can
/// assert that checked-in artefacts (the CHANGELOG, the published OpenAPI document) stay in step
/// with the code.
/// </summary>
public static class RepositoryPaths
{
    /// <summary>Walks up from the test assembly until it finds the repository root.</summary>
    public static string Root { get; } = FindRoot();

    public static string Changelog => Path.Combine(Root, "CHANGELOG.md");

    public static string OpenApiDocument => Path.Combine(Root, "docs", "public", "openapi.json");

    private static string FindRoot()
    {
        var directory = new DirectoryInfo(AppContext.BaseDirectory);

        while (directory is not null)
        {
            if (File.Exists(Path.Combine(directory.FullName, "CHANGELOG.md"))
                && Directory.Exists(Path.Combine(directory.FullName, "api")))
            {
                return directory.FullName;
            }

            directory = directory.Parent;
        }

        throw new InvalidOperationException(
            $"Could not locate the repository root above '{AppContext.BaseDirectory}'.");
    }
}
