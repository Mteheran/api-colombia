using api.Models;

namespace api.Tests.TestData;

internal static class CategoryNaturalAreaSeed
{
    public const int TotalRows = 3;

    private const string SharedDescription =
        "Área geográfica que, por poseer condiciones especiales de flora o gea es un escenario natural raro.";

    public static IReadOnlyList<CategoryNaturalArea> Rows() =>
    [
        new()
        {
            Id = 1,
            Name = "Área Natural Única",
            Description = SharedDescription
        },
        new()
        {
            Id = 2,
            Name = "Área Natural Protegida",
            Description = SharedDescription,
            NaturalAreas = new List<NaturalArea>()
        },
        new()
        {
            Id = 3,
            Name = "Área Natural de Interés Especial",
            Description = SharedDescription,
            NaturalAreas = new List<NaturalArea>()
        }
    ];
}
