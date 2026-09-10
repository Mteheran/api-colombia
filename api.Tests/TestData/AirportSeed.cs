using api.Models;

namespace api.Tests.TestData;

internal static class AirportSeed
{
    public const int TotalRows = 1;

    public static IReadOnlyList<Airport> Rows(CoreGraph core) =>
    [
        new()
        {
            Id = 1,
            Name = "Rionegro",
            CityId = core.Medellin.Id,
            City = core.Medellin,
            DeparmentId = core.Antioquia.Id,
            Department = core.Antioquia,
            IataCode = "MDE",
            OaciCode = "SKRG",
            Type = "Internacional"
        }
    ];
}
