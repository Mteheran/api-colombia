using api.Models;

namespace api.Tests.TestData;

internal static class TraditionalFairAndFestivalSeed
{
    public const int TotalRows = 3;

    /// <summary>
    /// Two festivals in Medellín, one in Cali, so `/{id}/city` can discriminate.
    ///
    /// Row 3 used to carry <c>CityId = 0</c> with no city — a state PostgreSQL forbids, since
    /// CityId is required and backed by a real foreign key. It survived only because the in-memory
    /// provider does not enforce constraints, and it left the API contradicting itself: the list
    /// endpoint`s Include(City) inner-joined the row away (2 rows) while pagedList still counted it
    /// (3 records).
    /// </summary>
    public static IReadOnlyList<TraditionalFairAndFestival> Rows(CoreGraph core) =>
    [
        new() { Id = 1, Name = "Sample Festival",  Description = "Sample description",  CityId = core.Medellin.Id, City = core.Medellin, Month = "January" },
        new() { Id = 2, Name = "Another Festival", Description = "Another description", CityId = core.Medellin.Id, City = core.Medellin, Month = "February" },
        new() { Id = 3, Name = "Super Event",      Description = "Third description",   CityId = core.Cali.Id,     City = core.Cali,     Month = "March" }
    ];
}
