using api.Models;

namespace api.Tests.TestData;

internal static class IntangibleHeritageSeed
{
    public const int TotalRows = 3;

    public static IReadOnlyList<IntangibleHeritage> Rows(CoreGraph core) =>
    [
        new() { Id = 1, Name = "Carnaval de Barranquilla", DepartmentId = core.Antioquia.Id, Department = core.Antioquia, Scope = "Nacional", InclusionYear = 2003 },
        new() { Id = 2, Name = "Fiesta de las Flores",     DepartmentId = core.Antioquia.Id, Department = core.Antioquia, Scope = "Regional", InclusionYear = 2010 },
        new() { Id = 3, Name = "Semana Santa",             DepartmentId = core.Amazonas.Id,  Department = core.Amazonas,  Scope = "Nacional", InclusionYear = 2012 }
    ];
}
