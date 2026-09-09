using System.Text.Json;
using api.Tests.Infrastructure;

namespace api.Tests.Integration.Resources;

/// <summary>
/// The real, observable pagination contract.
///
/// PaginationModel declares a custom BindAsync reading ?sortDir= and correcting a non-positive
/// page to 1, but every pagedList route takes it as [AsParameters], which binds each public
/// property individually and never calls BindAsync. So the live query keys are the property names
/// and the correction never happens. These tests pin what the service actually does, so that
/// wiring the binder up (or deleting it) shows here as a deliberate change.
/// </summary>
[Collection(SharedApiCollection.Name)]
public class PaginationBindingTests(ApiColombiaFactory factory) : IntegrationTestBase(factory)
{
    private const string Paged = "/api/v1/Volcano/pagedList";

    [Fact]
    public async Task SortDirection_IsTheKeyThatWorks()
    {
        var envelope = await _client.GetJsonAsync<JsonElement>(
            $"{Paged}?page=1&pagesize=10&sortBy=Name&sortDirection=desc");

        var names = envelope.GetProperty("data").EnumerateArray()
            .Select(r => r.GetProperty("name").GetString()!).ToList();

        Assert.Equal(names.OrderByDescending(n => n, StringComparer.CurrentCulture).ToList(), names);
    }

    /// <summary>
    /// ?sortDir= is the key BindAsync declares, and it is silently ignored — the rows come back in
    /// natural order and an unknown sortBy is not even validated.
    /// </summary>
    [Fact]
    public async Task SortDir_IsIgnored_BecauseBindAsyncNeverRuns()
    {
        var sorted = await _client.GetJsonAsync<JsonElement>(
            $"{Paged}?page=1&pagesize=10&sortBy=Name&sortDir=desc");
        var unsorted = await _client.GetJsonAsync<JsonElement>($"{Paged}?page=1&pagesize=10");

        Assert.Equal(unsorted.GetProperty("data").ToString(), sorted.GetProperty("data").ToString());
    }

    [Fact]
    public async Task SortDir_DoesNotEvenTriggerSortValidation() =>
        await _client.AssertStatusAsync(
            $"{Paged}?page=1&pagesize=10&sortBy=notAProperty&sortDir=asc",
            System.Net.HttpStatusCode.OK);

    [Fact]
    public async Task SortDirection_DoesTriggerSortValidation() =>
        await _client.AssertBadRequestAsync($"{Paged}?page=1&pagesize=10&sortBy=notAProperty&sortDirection=asc");

    [Theory]
    [InlineData("pagesize")]
    [InlineData("pageSize")]
    public async Task PageSize_BindsUnderEitherCasing(string key)
    {
        var envelope = await _client.GetJsonAsync<JsonElement>($"{Paged}?page=1&{key}=2");

        Assert.Equal(2, envelope.GetProperty("pageSize").GetInt32());
    }

    /// <summary>
    /// BindAsync would turn a non-positive page into 1; with it bypassed, the route's own guard
    /// rejects it instead. Uniform across every pagedList route, so it is a contract, not a bug.
    /// </summary>
    [Theory]
    [InlineData(0)]
    [InlineData(-5)]
    public async Task NonPositivePage_IsRejected_NotCorrected(int page) =>
        await _client.AssertBadRequestAsync($"{Paged}?page={page}&pagesize=10");
}
