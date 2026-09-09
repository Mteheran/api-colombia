using api.Models;

namespace api.Tests.TestData;

internal static class InvasiveSpecieSeed
{
    public const int TotalRows = 3;

    public static IReadOnlyList<InvasiveSpecie> Rows() =>
    [
        new()
        {
            Id = 1,
            Name = "Sample Specie",
            ScientificName = "Sample scientific name",
            CommonNames = "Common name 1, Common name 2",
            Impact = "Impact description",
            RiskLevel = RiskLevel.High,
            Manage = "Manage description",
            UrlImage = "https://example.com/image.jpg"
        },
        new()
        {
            Id = 2,
            Name = "Another Specie",
            ScientificName = "Another scientific name",
            CommonNames = "Common name 1, Common name 2",
            Impact = "Impact description",
            RiskLevel = RiskLevel.High,
            Manage = "Manage description",
            UrlImage = "https://example.com/image.jpg"
        },
        new()
        {
            Id = 3,
            Name = "Third Specie",
            ScientificName = "Third scientific name",
            CommonNames = "Common name 1, Common name 2",
            Impact = "Impact description",
            RiskLevel = RiskLevel.High,
            Manage = "Manage description",
            UrlImage = "https://example.com/image.jpg"
        }
    ];
}
