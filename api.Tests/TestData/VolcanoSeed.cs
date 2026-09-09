using api.Models;

namespace api.Tests.TestData;

internal static class VolcanoSeed
{
    public const int TotalRows = 3;

    /// <summary>Two volcanoes in Antioquia/Medellín, one in Amazonas/Cali, with distinct elevations for sort tests.</summary>
    public static IReadOnlyList<Volcano> Rows(CoreGraph core) =>
    [
        new()
        {
            Id = 1,
            Name = "Volcán Nevado del Ruiz",
            Description = "Estratovolcán activo del Parque Nacional Natural Los Nevados.",
            Elevation = 5321,
            Latitude = 4.895,
            Longitude = -75.321,
            VolcanoType = "Estratovolcán o Volcán compuesto",
            ActivityLevel = "Alerta amarilla",
            ImageUrl = "https://example.com/ruiz.jpg",
            DepartmentId = core.Antioquia.Id,
            Department = core.Antioquia,
            CityId = core.Medellin.Id,
            City = core.Medellin
        },
        new()
        {
            Id = 2,
            Name = "Volcán Cerro Machín",
            Description = "Complejo anillo piroclástico-domo de alta explosividad.",
            Elevation = 2750,
            Latitude = 4.487,
            Longitude = -75.386,
            VolcanoType = "Complejo anillo piroclástico-domo",
            ActivityLevel = "Alerta amarilla",
            ImageUrl = "https://example.com/machin.jpg",
            DepartmentId = core.Antioquia.Id,
            Department = core.Antioquia,
            CityId = core.Medellin.Id,
            City = core.Medellin
        },
        new()
        {
            Id = 3,
            Name = "Volcán Galeras",
            Description = "Volcán ubicado al occidente de la ciudad de Pasto.",
            Elevation = 4276,
            Latitude = 1.221,
            Longitude = -77.359,
            VolcanoType = "Estratovolcán o Volcán compuesto",
            ActivityLevel = "Alerta amarilla",
            ImageUrl = "https://example.com/galeras.jpg",
            DepartmentId = core.Amazonas.Id,
            Department = core.Amazonas,
            CityId = core.Cali.Id,
            City = core.Cali
        }
    ];
}
