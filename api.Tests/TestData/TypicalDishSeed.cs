using api.Models;

namespace api.Tests.TestData;

internal static class TypicalDishSeed
{
    public const int TotalRows = 3;

    /// <summary>Two dishes in Antioquia, one in Amazonas, so `/{id}/department` can discriminate.</summary>
    public static IReadOnlyList<TypicalDish> Rows(CoreGraph core) =>
    [
        new()
        {
            Id = 1,
            Name = "Bandeja Paisa",
            Description = "Traditional dish from Antioquia",
            Ingredients = "Rice, beans, meat, avocado, plantain",
            DepartmentId = core.Antioquia.Id,
            Department = core.Antioquia,
            ImageUrl = "https://example.com/bandeja_paisa.jpg"
        },
        new()
        {
            Id = 2,
            Name = "Arepa",
            Description = "Corn cake",
            Ingredients = "Corn flour, water, salt",
            DepartmentId = core.Antioquia.Id,
            Department = core.Antioquia,
            ImageUrl = "https://example.com/arepa.jpg"
        },
        new()
        {
            Id = 3,
            Name = "Sancocho",
            Description = "Traditional soup",
            Ingredients = "Meat, plantain, yucca, corn",
            DepartmentId = core.Amazonas.Id,
            Department = core.Amazonas,
            ImageUrl = "https://example.com/sancocho.jpg"
        }
    ];
}
