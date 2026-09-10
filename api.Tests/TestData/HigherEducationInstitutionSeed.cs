using api.Models;

namespace api.Tests.TestData;

internal static class HigherEducationInstitutionSeed
{
    public const int TotalRows = 3;

    public static IReadOnlyList<HigherEducationInstitution> Rows(CoreGraph core) =>
    [
        new()
        {
            Id = 1,
            Code = "1101",
            Name = "Universidad de Antioquia",
            LegalNature = "Departamental",
            AcademicCharacter = "Universidad",
            CityId = core.Medellin.Id,
            City = core.Medellin,
            Address = "Calle 67 No. 53-108",
            Phone = "6042198332",
            IsHighQualityAccredited = true,
            Website = "www.udea.edu.co"
        },
        new()
        {
            Id = 2,
            Code = "1201",
            Name = "Institución Universitaria Pascual Bravo",
            LegalNature = "Municipal",
            AcademicCharacter = "Institución Universitaria/Escuela Tecnológica",
            CityId = core.Medellin.Id,
            City = core.Medellin,
            Address = "Calle 73 No. 73A-226",
            Phone = "6044480520",
            IsHighQualityAccredited = false,
            Website = "www.pascualbravo.edu.co"
        },
        new()
        {
            Id = 3,
            Code = "1801",
            Name = "Universidad del Valle",
            LegalNature = "Departamental",
            AcademicCharacter = "Universidad",
            CityId = core.Cali.Id,
            City = core.Cali,
            Address = "Ciudad Universitaria Meléndez Calle 13 No. 100-00",
            Phone = "6023212100",
            IsHighQualityAccredited = true,
            Website = "www.univalle.edu.co"
        }
    ];
}
