using System.Text.RegularExpressions;
using api.Const;
using api.Tests.Infrastructure;

namespace api.Tests.Unit;

/// <summary>
/// The version shown in Swagger comes from <see cref="VersionInfo.CurrentVersion"/>, and the
/// release notes come from CHANGELOG.md. Nothing kept the two in step, so a release could ship
/// with Swagger advertising one version and the changelog documenting another.
/// </summary>
public partial class VersionConsistencyTests
{
    [GeneratedRegex(@"^## \[(\d+\.\d+\.\d+)\] - (\d{4}-\d{2}-\d{2})\s*$", RegexOptions.Multiline)]
    private static partial Regex ReleaseHeading();

    [GeneratedRegex(@"^\[(\d+\.\d+\.\d+)\]: (\S+)\s*$", RegexOptions.Multiline)]
    private static partial Regex ReleaseLink();

    private const string RepositoryUrl = "https://github.com/Mteheran/api-colombia";

    private static string Changelog => File.ReadAllText(RepositoryPaths.Changelog);

    [Fact]
    public void SwaggerVersion_MatchesTheNewestChangelogEntry()
    {
        var newest = ReleaseHeading().Match(Changelog);

        Assert.True(newest.Success, "CHANGELOG.md has no '## [x.y.z] - YYYY-MM-DD' release heading.");
        Assert.Equal(newest.Groups[1].Value, VersionInfo.CurrentVersion);
    }

    [Fact]
    public void Changelog_ListsReleasesNewestFirst()
    {
        var versions = ReleaseHeading().Matches(Changelog)
            .Select(m => new Version(m.Groups[1].Value))
            .ToList();

        Assert.Equal(versions.OrderByDescending(v => v).ToList(), versions);
    }

    [Fact]
    public void EveryReleaseHeading_HasAReleaseLink()
    {
        var headings = ReleaseHeading().Matches(Changelog).Select(m => m.Groups[1].Value).ToHashSet();
        var linked = ReleaseLink().Matches(Changelog).Select(m => m.Groups[1].Value).ToHashSet();

        Assert.Empty(headings.Except(linked));
        Assert.Empty(linked.Except(headings));
    }

    /// <summary>
    /// The oldest entries kept the Keep a Changelog template's placeholder — every 1.0.x link
    /// pointed at github.com/Author/Repository, which does not exist.
    /// </summary>
    [Fact]
    public void EveryReleaseLink_PointsAtThisRepository()
    {
        Assert.All(ReleaseLink().Matches(Changelog),
            m => Assert.StartsWith(RepositoryUrl, m.Groups[2].Value));
    }

    [Fact]
    public void EveryReleaseLink_EndsAtItsOwnVersion()
    {
        Assert.All(ReleaseLink().Matches(Changelog),
            m => Assert.EndsWith($"v{m.Groups[1].Value}", m.Groups[2].Value));
    }
}
