using System.Text.Json;
using api.Tests.Infrastructure;

namespace api.Tests.Integration.Resources;

/// <summary>
/// The observable pagination contract.
///
/// PaginationModel is bound with [AsParameters], so the query keys are its property names. It used
/// to also declare a custom BindAsync reading a ?sortDir= key and correcting a non-positive page to
/// 1 — [AsParameters] never calls BindAsync, so none of that ran, and the dead binder has been
/// removed. These tests pin the keys that actually work so a future change to the binding shows up
/// here rather than silently in production.
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
    /// ?sortDir= — the key the removed binder declared — is not a parameter of this API. It is
    /// ignored: the rows come back in natural order and an unknown sortBy is not even validated.
    /// </summary>
    [Fact]
    public async Task SortDir_IsNotAParameter_AndIsIgnored()
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
    /// A non-positive page is rejected by each route's own guard rather than corrected to 1.
    /// Uniform across every pagedList route, so it is the contract, not an accident.
    /// </summary>
    [Theory]
    [InlineData(0)]
    [InlineData(-5)]
    public async Task NonPositivePage_IsRejected_NotCorrected(int page) =>
        await _client.AssertBadRequestAsync($"{Paged}?page={page}&pagesize=10");
}
