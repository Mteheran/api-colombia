namespace api.Tests.Infrastructure;

/// <summary>
/// Base for every resource test: guarantees the shared database is seeded and hands out a client.
/// Subclasses still declare <c>[Collection(SharedApiCollection.Name)]</c> explicitly — xUnit
/// resolves collections per concrete class, so the attribute is not inherited.
/// </summary>
public abstract class IntegrationTestBase
{
    protected readonly HttpClient _client;

    protected IntegrationTestBase(ApiColombiaFactory factory)
    {
        factory.EnsureSeeded();
        _client = factory.CreateClient();
    }
}
