using api.Models;

namespace api.Tests.TestData;

internal static class TouristAttractionSeed
{
    public const int TotalRows = 3;

    private static readonly string[] SampleImages =
        ["https://example.com/image1.jpg", "https://example.com/image2.jpg"];

    public static IReadOnlyList<TouristAttraction> Rows(CoreGraph core)
    {
        var explora = new TouristAttraction
        {
            Id = 1,
            Name = "Parque Explora",
            Description = "Parque temático de ciencia y tecnología",
            City = core.Medellin,
            CityId = core.Medellin.Id
        };

        core.Medellin.TouristAttractions = new List<TouristAttraction> { explora };

        return
        [
            explora,
            new()
            {
                Id = 2,
                Name = "Pueblito paisa",
                Description = "Lugar emblematico de Medellin",
                CityId = core.Medellin.Id,
                City = core.Medellin,
                Images = SampleImages
            },
            new()
            {
                Id = 3,
                Name = "Parque Norte",
                Description = "Parque temático de ciencia y tecnología",
                CityId = core.Medellin.Id,
                City = core.Medellin,
                Images = SampleImages
            }
        ];
    }
}
