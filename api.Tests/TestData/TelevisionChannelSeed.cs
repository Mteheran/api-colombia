using api.Models;

namespace api.Tests.TestData;

internal static class TelevisionChannelSeed
{
    public const int TotalRows = 3;

    /// <summary>
    /// Row 2 has a null Url and IsActive = false; row 3 ("Canal RCN") exists so the
    /// short-keyword search path — the only caller passing <c>allowShortKeywords: true</c> —
    /// has an acronym to match.
    /// </summary>
    public static IReadOnlyList<TelevisionChannel> Rows(CoreGraph core) =>
    [
        new() { Id = 1, Name = "Sample TV Channel",  CityId = core.Medellin.Id, City = core.Medellin, Url = new Uri("https://example.com/tv"),  IsActive = true },
        new() { Id = 2, Name = "Another TV Channel", CityId = core.Medellin.Id, City = core.Medellin, Url = null!,                              IsActive = false },
        new() { Id = 3, Name = "Canal RCN",          CityId = core.Cali.Id,     City = core.Cali,     Url = new Uri("https://example.com/tv3"), IsActive = true }
    ];
}
