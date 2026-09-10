using api.Utils;

namespace api.Tests.Unit;

/// <summary>
/// <see cref="Functions.ApplySorting{T}"/> backs the ?sortBy/?sortDirection contract on every list
/// and pagedList route in the API, and had no direct test — its behaviour was only ever observed
/// through HTTP.
/// </summary>
public class FunctionsSortingTests
{
    private sealed record Row(int Id, string Name);

    private static readonly IQueryable<Row> Rows = new[]
    {
        new Row(1, "Charlie"),
        new Row(2, "alpha"),
        new Row(3, "Bravo"),
    }.AsQueryable();

    [Fact]
    public void SortsAscending()
    {
        var (sorted, isValid) = Functions.ApplySorting(Rows, "Name", "asc");

        Assert.True(isValid);
        Assert.Equal(Rows.OrderBy(r => r.Name).Select(r => r.Id), sorted.Select(r => r.Id));
    }

    [Fact]
    public void SortsDescending()
    {
        var (sorted, isValid) = Functions.ApplySorting(Rows, "Name", "desc");

        Assert.True(isValid);
        Assert.Equal(Rows.OrderByDescending(r => r.Name).Select(r => r.Id), sorted.Select(r => r.Id));
    }

    [Theory]
    [InlineData("name")]
    [InlineData("NAME")]
    [InlineData("nAmE")]
    public void PropertyNameIsCaseInsensitive(string sortBy)
    {
        var (_, isValid) = Functions.ApplySorting(Rows, sortBy, "asc");

        Assert.True(isValid);
    }

    [Theory]
    [InlineData("asc")]
    [InlineData("Asc")]
    [InlineData("DESC")]
    public void DirectionIsCaseInsensitive(string direction)
    {
        var (_, isValid) = Functions.ApplySorting(Rows, "Name", direction);

        Assert.True(isValid);
    }

    [Fact]
    public void UnknownProperty_IsRejected()
    {
        var (sorted, isValid) = Functions.ApplySorting(Rows, "NotAProperty", "asc");

        Assert.False(isValid);
        Assert.Equal(Rows.Select(r => r.Id), sorted.Select(r => r.Id));
    }

    [Fact]
    public void UnknownDirection_IsRejected()
    {
        var (_, isValid) = Functions.ApplySorting(Rows, "Name", "sideways");

        Assert.False(isValid);
    }

    /// <summary>
    /// Supplying only one half of the pair is accepted and silently leaves the query unsorted —
    /// `?sortBy=Name` with no direction returns 200 in source order rather than 400.
    /// </summary>
    [Theory]
    [InlineData("Name", "")]
    [InlineData("", "asc")]
    [InlineData("", "")]
    [InlineData("NotAProperty", "")]
    public void IncompletePair_IsAcceptedAndLeavesTheQueryUnsorted(string sortBy, string direction)
    {
        var (sorted, isValid) = Functions.ApplySorting(Rows, sortBy, direction);

        Assert.True(isValid);
        Assert.Equal(Rows.Select(r => r.Id), sorted.Select(r => r.Id));
    }

    [Fact]
    public void SortsOnNonStringProperties()
    {
        var (sorted, isValid) = Functions.ApplySorting(Rows, "Id", "desc");

        Assert.True(isValid);
        Assert.Equal([3, 2, 1], sorted.Select(r => r.Id));
    }
}
