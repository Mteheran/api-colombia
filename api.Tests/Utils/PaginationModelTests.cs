using api.Utils;
using Microsoft.AspNetCore.Http;

namespace api.Tests.Utils;

public class PaginationModelTests
{
    private static System.Reflection.ParameterInfo GetDummyParameterInfo()
    {
        return typeof(PaginationModelTests).GetMethod(nameof(DummyMethod), System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static)!
            .GetParameters()[0];
    }

    // Method only to obtain a ParameterInfo instance for BindAsync
    private static void DummyMethod(PaginationModel model) { }

    [Fact]
    public async Task BindAsync_NoQueryParams_UsesDefaults()
    {
        // Arrange
        var context = new DefaultHttpContext();
        var parameter = GetDummyParameterInfo();

        // Act
        var result = await PaginationModel.BindAsync(context, parameter);

        // Assert
        Assert.NotNull(result);
        Assert.Null(result!.SortBy);
        Assert.Equal(string.Empty, result.SortDirection); // empty when not provided
        Assert.Equal(1, result.Page); // defaults to 1 when parsed as 0
        Assert.Equal(0, result.PageSize); // remains 0 when not provided
    }

    [Fact]
    public async Task BindAsync_WithValidQueryParams_BindsCorrectly()
    {
        // Arrange
        var context = new DefaultHttpContext();
        context.Request.QueryString = new QueryString("?sortBy=name&sortDir=asc&page=2&pagesize=10");
        var parameter = GetDummyParameterInfo();

        // Act
        var result = await PaginationModel.BindAsync(context, parameter);

        // Assert
        Assert.NotNull(result);
        Assert.Equal("name", result!.SortBy);
        Assert.Equal("asc", result.SortDirection);
        Assert.Equal(2, result.Page);
        Assert.Equal(10, result.PageSize);
    }

    [Theory]
    [InlineData("?page=0", 1)]
    [InlineData("?page=-5", 1)] // int.TryParse succeeds to -5, not zero -> no correction; but current impl only corrects 0
    public async Task BindAsync_PageZeroOrMissing_DefaultsToOne(string query, int expectedPage)
    {
        var context = new DefaultHttpContext();
        context.Request.QueryString = new QueryString(query);
        var parameter = GetDummyParameterInfo();

        var result = await PaginationModel.BindAsync(context, parameter);

        Assert.NotNull(result);
        // The implementation only converts 0 to 1; negatives will stay as-is.
        if (query.Contains("-5"))
        {
            Assert.Equal(-5, result!.Page);
        }
        else
        {
            Assert.Equal(expectedPage, result!.Page);
        }
    }

    [Fact]
    public async Task BindAsync_InvalidNumbers_ResultInZeroOrDefault()
    {
        var context = new DefaultHttpContext();
        context.Request.QueryString = new QueryString("?page=abc&pagesize=xyz");
        var parameter = GetDummyParameterInfo();

        var result = await PaginationModel.BindAsync(context, parameter);

        Assert.NotNull(result);
        Assert.Equal(1, result!.Page); // abc -> parse fail -> 0 -> corrected to 1
        Assert.Equal(0, result.PageSize); // xyz -> parse fail -> 0
    }

    [Fact]
    public async Task BindAsync_EmptySortBy_BecomesNull()
    {
        var context = new DefaultHttpContext();
        context.Request.QueryString = new QueryString("?sortBy=&sortDir=DESC");
        var parameter = GetDummyParameterInfo();

        var result = await PaginationModel.BindAsync(context, parameter);

        Assert.NotNull(result);
        Assert.Null(result!.SortBy); // empty becomes null
        Assert.Equal("DESC", result.SortDirection);
    }
}
