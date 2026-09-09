using api.Models;

namespace api.Tests.TestData;

internal static class HeritageCitySeed
{
    public const int TotalRows = 3;

    public static IReadOnlyList<HeritageCity> Rows(CoreGraph core) =>
    [
        new()
        {
            Id = 1,
            Name = "Cartagena",
            Description = "Historic walled city with colonial architecture.",
            CityId = core.Medellin.Id,
            City = core.Medellin,
            DepartmentId = core.Antioquia.Id,
            Department = core.Antioquia,
            Image = "https://example.com/cartagena.jpg"
        },
        new()
        {
            Id = 2,
            Name = "Popayan",
            Description = "Colonial city known for its white architecture.",
            CityId = core.Medellin.Id,
            City = core.Medellin,
            DepartmentId = core.Antioquia.Id,
            Department = core.Antioquia,
            Image = "https://example.com/popayan.jpg"
        },
        new()
        {
            Id = 3,
            Name = "Santa Cruz de Mompox",
            Description = "River town with preserved colonial heritage.",
            CityId = core.Medellin.Id,
            City = core.Medellin,
            DepartmentId = core.Antioquia.Id,
            Department = core.Antioquia,
            Image = "https://example.com/mompox.jpg"
        }
    ];
}
