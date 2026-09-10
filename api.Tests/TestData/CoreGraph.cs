using api.Models;

namespace api.Tests.TestData;

/// <summary>
/// The Region → Department → City graph that every other seeder hangs off.
///
/// Built fresh for each seeding run: EF Core tracks these instances, so they must never
/// be shared between <see cref="DBContext"/> instances.
///
/// Everything here is inserted explicitly by <see cref="TestSeeder"/>. It used to be that
/// only Antioquia and Medellín were added, and Amazonas/Cali reached the database purely as
/// a side effect of the navigation properties of TypicalDish #3, PostalCode #3, UrbanCenter #3,
/// HigherEducationInstitution #3, TelevisionChannel #3 and Volcano #3 — so deleting any one of
/// those blocks silently removed a department and a city from unrelated tests.
/// </summary>
internal sealed class CoreGraph
{
    public const int RegionRows = 1;
    public const int DepartmentRows = 2;
    public const int CityRows = 2;

    /// <summary>The only region. Both departments belong to it.</summary>
    public Region Andina { get; }

    /// <summary>Department 1 — owns most of the seeded rows.</summary>
    public Department Antioquia { get; }

    /// <summary>Department 10 — the "other" department, so per-department filters can discriminate.</summary>
    public Department Amazonas { get; }

    /// <summary>City 1, capital of <see cref="Antioquia"/>.</summary>
    public City Medellin { get; }

    /// <summary>City 20, the "other" city, so per-city filters can discriminate.</summary>
    public City Cali { get; }

    public CoreGraph()
    {
        Medellin = new City { Id = 1, Name = "Medellín", DepartmentId = 1 };
        Cali = new City { Id = 20, Name = "Cali" };

        Andina = new Region
        {
            Id = 1,
            Name = "Andina",
            Description = "Región natural de Colombia",
            Departments = new List<Department>()
        };

        Antioquia = new Department
        {
            Id = 1,
            Name = "Antioquia",
            NaturalAreas = new List<NaturalArea>(),
            CityCapitalId = 1,
            CityCapital = Medellin,
            Region = Andina,
            RegionId = Andina.Id,
            Cities = new List<City> { Medellin }
        };

        Amazonas = new Department
        {
            Id = 10,
            Name = "Amazonas",
            NaturalAreas = new List<NaturalArea>(),
            CityCapitalId = 1,
            CityCapital = Cali,
            Region = Andina,
            RegionId = Andina.Id,
            Cities = new List<City> { Cali }
        };

        Andina.Departments = new List<Department> { Antioquia, Amazonas };
        Medellin.Department = Antioquia;
        Cali.Department = Amazonas;
        Cali.DepartmentId = Amazonas.Id;
    }

    /// <summary>Every core entity, for explicit insertion.</summary>
    public IEnumerable<object> All => [Andina, Antioquia, Amazonas, Medellin, Cali];
}
