using api.Models;

namespace api.Tests.TestData;

internal static class PresidentSeed
{
    public const int TotalRows = 3;

    /// <summary>
    /// President 2 deliberately has no <c>EndPeriodDate</c>: `/President/year/{year}` treats an
    /// open period as running up to the current year, and that branch needs a row to hit.
    /// </summary>
    public static IReadOnlyList<President> Rows(CoreGraph core) =>
    [
        new()
        {
            Id = 1,
            Name = "Rafael",
            LastName = "Núñez",
            Description = "First in Colombia",
            Image = "https://example.com/image.jpg",
            PoliticalParty = "Sample Party",
            StartPeriodDate = new DateOnly(1865, 1, 1),
            EndPeriodDate = new DateOnly(1866, 1, 1),
            CityId = core.Medellin.Id,
            City = core.Medellin
        },
        new()
        {
            Id = 2,
            Name = "Another President",
            LastName = "Another",
            StartPeriodDate = new DateOnly(1866, 1, 1),
            PoliticalParty = "Sample Party",
            Description = "President of Colombia",
            Image = "https://example.com/image.jpg",
            CityId = core.Medellin.Id,
            City = core.Medellin
        },
        new()
        {
            Id = 3,
            Name = "Third President",
            LastName = "Third",
            StartPeriodDate = new DateOnly(1867, 1, 1),
            EndPeriodDate = new DateOnly(1868, 1, 1),
            PoliticalParty = "Sample Party",
            Description = "President of Colombia",
            Image = "https://example.com/image.jpg",
            CityId = core.Medellin.Id,
            City = core.Medellin
        }
    ];
}
