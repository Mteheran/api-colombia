using api.Models;

namespace api.Tests.TestData;

internal static class PostalCodeSeed
{
    public const int TotalRows = 3;

    public static IReadOnlyList<PostalCode> Rows(CoreGraph core) =>
    [
        new()
        {
            Id = 1,
            NoId = 101,
            CityId = core.Medellin.Id,
            City = core.Medellin,
            PostalZone = "Urban Zone 1",
            Code = "110111",
            NorthLimit = "Street 100",
            SouthLimit = "Street 80",
            EastLimit = "Avenue 9",
            WestLimit = "Avenue 30",
            Type = "Urban",
            NeighborhoodsContainedInPostalCode = "Chapinero, El Nogal",
            RuralAreasContainedInPostalCode = ""
        },
        new()
        {
            Id = 2,
            NoId = 102,
            CityId = core.Medellin.Id,
            City = core.Medellin,
            PostalZone = "Urban Zone 2",
            Code = "110112",
            NorthLimit = "Street 79",
            SouthLimit = "Street 60",
            EastLimit = "Avenue 7",
            WestLimit = "Avenue 24",
            Type = "Urban",
            NeighborhoodsContainedInPostalCode = "Teusaquillo, Galerias",
            RuralAreasContainedInPostalCode = ""
        },
        new()
        {
            Id = 3,
            NoId = 103,
            CityId = core.Cali.Id,
            City = core.Cali,
            PostalZone = "Rural Zone 1",
            Code = "050001",
            NorthLimit = "Rural North",
            SouthLimit = "Rural South",
            EastLimit = "Rural East",
            WestLimit = "Rural West",
            Type = "Rural",
            NeighborhoodsContainedInPostalCode = "",
            RuralAreasContainedInPostalCode = "San Cristobal, Altavista"
        }
    ];
}
