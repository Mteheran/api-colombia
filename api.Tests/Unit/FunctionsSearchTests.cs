using api.Utils;

namespace api.Tests.Unit;

/// <summary>
/// <see cref="Functions.FilterObjectListPropertiesByKeyword{T}"/> powers every /search/{keyword}
/// route. Callers normalise the keyword themselves (trim + upper-case) before calling, so these
/// tests pass an already-normalised keyword the way the routes do.
/// </summary>
public class FunctionsSearchTests
{
    private sealed record Row(int Id, string Name, string Description, string ImagesUrl);

    private static List<Row> Rows() =>
    [
        new(1, "Bogotá", "Capital district", "https://example.com/a.jpg"),
        new(2, "Medellín", "City of eternal spring", "https://example.com/b.jpg"),
        new(3, "RCN", "A short acronym", "https://example.com/c.jpg"),
    ];

    [Theory]
    [InlineData("")]
    [InlineData(null)]
    public void EmptyKeyword_MatchesNothing(string? keyword)
    {
        var result = Functions.FilterObjectListPropertiesByKeyword(Rows(), keyword!);

        Assert.Empty(result);
    }

    [Fact]
    public void MatchesASubstringOfALongKeyword()
    {
        var result = Functions.FilterObjectListPropertiesByKeyword(Rows(), "ETERNAL");

        Assert.Equal([2], result.Select(r => r.Id));
    }

    [Fact]
    public void IgnoresAccents()
    {
        // "Bogotá" is stored with an accent; the keyword has none.
        var result = Functions.FilterObjectListPropertiesByKeyword(Rows(), "BOGOTA");

        Assert.Equal([1], result.Select(r => r.Id));
    }

    /// <summary>
    /// Keywords of three characters or fewer would match almost every long description, so they
    /// only match a whole field unless the caller opts in.
    /// </summary>
    [Fact]
    public void ShortKeyword_OnlyMatchesAWholeField_ByDefault()
    {
        var result = Functions.FilterObjectListPropertiesByKeyword(Rows(), "RCN");

        Assert.Equal([3], result.Select(r => r.Id));
    }

    [Fact]
    public void ShortKeyword_MatchesNothingWhenItIsOnlyASubstring()
    {
        // "ACR" appears inside "A short acronym" but is too short to be searched as a substring.
        var result = Functions.FilterObjectListPropertiesByKeyword(Rows(), "ACR");

        Assert.Empty(result);
    }

    [Fact]
    public void ShortKeyword_MatchesSubstrings_WhenOptedIn()
    {
        var result = Functions.FilterObjectListPropertiesByKeyword(Rows(), "ACR", allowShortKeywords: true);

        Assert.Equal([3], result.Select(r => r.Id));
    }

    /// <summary>Image URLs are excluded so a search never matches on a CDN path.</summary>
    [Fact]
    public void PropertiesNamedImages_AreNotSearched()
    {
        var result = Functions.FilterObjectListPropertiesByKeyword(Rows(), "EXAMPLE.COM");

        Assert.Empty(result);
    }

    [Fact]
    public void ARowMatchingSeveralProperties_IsReturnedOnce()
    {
        List<Row> rows = [new(1, "REPEATED", "REPEATED", "n/a")];

        var result = Functions.FilterObjectListPropertiesByKeyword(rows, "REPEATED");

        Assert.Single(result);
    }

    [Fact]
    public void KeywordMustAlreadyBeUpperCased_CasingIsTheCallersJob()
    {
        // Every route calls keyword.Trim().ToUpper() before delegating here; a lower-case keyword
        // reaches the comparison unchanged and finds nothing.
        var result = Functions.FilterObjectListPropertiesByKeyword(Rows(), "eternal");

        Assert.Empty(result);
    }
}
