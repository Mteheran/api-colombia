using api.Models;

namespace api.Tests.TestData;

internal static class IndigenousReservationSeed
{
    public const int TotalRows = 3;

    public static IReadOnlyList<IndigenousReservation> Rows(CoreGraph core) =>
    [
        new() { Id = 1, Name = "Emberá", CityId = core.Medellin.Id, City = core.Medellin, Department = core.Antioquia, DeparmentId = core.Antioquia.Id },
        new() { Id = 2, Name = "Wayuu",  CityId = core.Medellin.Id, City = core.Medellin, Department = core.Antioquia, DeparmentId = core.Antioquia.Id },
        new() { Id = 3, Name = "Zenú",   CityId = core.Medellin.Id, City = core.Medellin, Department = core.Antioquia, DeparmentId = core.Antioquia.Id }
    ];
}
