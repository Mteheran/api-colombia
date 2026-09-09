using api.Models;

namespace api.Tests.TestData;

internal static class MapSeed
{
    public const int TotalRows = 2;

    private static readonly string[] SampleImages =
    [
        "https://upload.wikimedia.org/wikipedia/commons/thumb/2/2c/Colombia_%28orthographic_projection%29.svg/1200px-Colombia_%28orthographic_projection%29.svg.png",
        "https://upload.wikimedia.org/wikipedia/commons/thumb/4/4f/Colombia_location_map.svg/1200px-Colombia_location_map.svg.png"
    ];

    public static IReadOnlyList<Map> Rows() =>
    [
        new()
        {
            Id = 1,
            Name = "Mapa de Colombia",
            Description = "Mapa de Colombia",
            UrlSource = "https://www.google.com/maps/place/Colombia/@4.570868,-74.297333,5z",
            UrlImages = SampleImages
        },
        new()
        {
            Id = 2,
            Name = "Mapa de Medellín",
            Description = "Mapa de Medellín",
            UrlSource = "https://www.google.com/maps/place/Medell%C3%ADn,+Antioquia/@6.244203,-75.590654,12z",
            UrlImages = SampleImages
        }
    ];
}
