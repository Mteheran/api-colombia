using api.Models;

namespace api.Tests.TestData;

internal static class TraditionalFairAndFestivalSeed
{
    public const int TotalRows = 3;

    /// <summary>
    /// Row 3 is deliberately orphaned (<c>CityId = 0</c>, no city) so tests can exercise a
    /// festival that no `/{id}/city` lookup will ever return.
    /// </summary>
    public static IReadOnlyList<TraditionalFairAndFestival> Rows(CoreGraph core) =>
    [
        new() { Id = 1, Name = "Sample Festival",  Description = "Sample description",  CityId = core.Medellin.Id, City = core.Medellin, Month = "January" },
        new() { Id = 2, Name = "Another Festival", Description = "Another description", CityId = core.Medellin.Id, City = core.Medellin, Month = "February" },
        new() { Id = 3, Name = "Super Event",      Description = "Third description",   CityId = 0,                City = null!,         Month = "March" }
    ];
}
