using api.Models;

namespace api.Tests.TestData;

internal static class NaturalAreaSeed
{
    public const int TotalRows = 1;

    /// <summary>
    /// The single natural area, wired into both its category and its department so the
    /// `/CategoryNaturalArea/{id}/NaturalAreas` and `/Department/{id}/naturalareas` routes
    /// have something to return.
    /// </summary>
    public static IReadOnlyList<NaturalArea> Rows(CoreGraph core, IReadOnlyList<CategoryNaturalArea> categories)
    {
        var category = categories[0];

        var arvi = new NaturalArea
        {
            Id = 1,
            Name = "Parque Arví",
            DepartmentId = core.Antioquia.Id,
            CategoryNaturalAreaId = category.Id,
            CategoryNaturalArea = category
        };

        category.NaturalAreas = new List<NaturalArea> { arvi };
        core.Antioquia.NaturalAreas = new List<NaturalArea> { arvi };

        return [arvi];
    }
}
