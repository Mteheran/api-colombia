using api.Models;

namespace api.Tests.TestData;

internal static class UrbanCenterSeed
{
    public const int TotalRows = 3;

    public static IReadOnlyList<UrbanCenter> Rows(CoreGraph core) =>
    [
        new() { Id = 1, CityId = core.Medellin.Id, City = core.Medellin, Code = "001", Name = "Centro Urbano 1",  Type = "Municipal Head",   Longitude = -74.0, Latitude = 4.0 },
        new() { Id = 2, CityId = core.Medellin.Id, City = core.Medellin, Code = "002", Name = "Centro Poblado 1", Type = "Populated Center", Longitude = -74.1, Latitude = 4.1 },
        new() { Id = 3, CityId = core.Cali.Id,     City = core.Cali,     Code = "003", Name = "Centro Urbano 2",  Type = "Municipal Head",   Longitude = -75.0, Latitude = 6.0 }
    ];
}
