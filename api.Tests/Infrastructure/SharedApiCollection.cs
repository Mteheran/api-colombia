namespace api.Tests.Infrastructure;

/// <summary>
/// Groups every read-only resource test onto one seeded host. xUnit runs the classes in a
/// collection sequentially, which is the trade for booting and seeding only once.
/// </summary>
[CollectionDefinition(Name)]
public sealed class SharedApiCollection : ICollectionFixture<ApiColombiaFactory>
{
    public const string Name = "Shared API";
}
