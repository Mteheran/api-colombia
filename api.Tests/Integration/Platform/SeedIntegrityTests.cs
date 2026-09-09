using api.Tests.Infrastructure;
using api.Tests.TestData;
using Microsoft.Extensions.DependencyInjection;

namespace api.Tests.Integration.Platform;

/// <summary>
/// Guards the seed itself.
///
/// The seed used to insert only Antioquia and Medellín explicitly; Amazonas and Cali reached the
/// database purely as a side effect of the navigation properties of six unrelated resources, so
/// removing any one of those blocks would have silently deleted a department and a city out from
/// under other resources' tests. Asserting every table against its declared row count makes that
/// class of accident a failure instead of a mystery.
/// </summary>
[Collection(SharedApiCollection.Name)]
public class SeedIntegrityTests(ApiColombiaFactory factory) : IntegrationTestBase(factory)
{
    private readonly ApiColombiaFactory _factory = factory;

    public static TheoryData<string> SeededTables() => [.. TestSeeder.ExpectedRowCounts.Keys];

    [Theory]
    [MemberData(nameof(SeededTables))]
    public void SeededTable_HasExactlyTheDeclaredNumberOfRows(string table)
    {
        var (expected, actual) = TestSeeder.ExpectedRowCounts[table];

        using var scope = _factory.Services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<DBContext>();

        Assert.Equal(expected, actual(db));
    }

    [Fact]
    public void BothDepartments_AreReachable_SoPerDepartmentFiltersCanDiscriminate()
    {
        using var scope = _factory.Services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<DBContext>();

        Assert.Contains(db.Departments, d => d.Name == "Antioquia");
        Assert.Contains(db.Departments, d => d.Name == "Amazonas");
        Assert.Contains(db.Cities, c => c.Name == "Medellín");
        Assert.Contains(db.Cities, c => c.Name == "Cali");
    }
}
