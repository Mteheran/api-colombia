using api.Mcp;
using api.Tests.TestData;
using Microsoft.EntityFrameworkCore;

namespace api.Tests.Unit;

/// <summary>
/// ResourceCatalog is the single source of truth behind every MCP data tool: adding a resource
/// here exposes it through all of them. These tests reach the argument-handling branches the MCP
/// protocol cannot produce — a null or blank key never survives JSON-RPC parameter binding — and
/// pin the catalog's shape so a resource cannot be added without a route and a description.
/// </summary>
public class ResourceCatalogTests
{
    private static DBContext NewSeededContext()
    {
        var options = new DbContextOptionsBuilder<DBContext>()
            .UseInMemoryDatabase($"CatalogTests_{Guid.NewGuid()}")
            .Options;

        var db = new DBContext(options);
        TestSeeder.Seed(db);
        return db;
    }

    private static ResourceDescriptor Get(string key)
    {
        Assert.True(ResourceCatalog.TryGet(key, out var descriptor), $"'{key}' should be a catalog key.");
        return descriptor;
    }

    // ---------- TryGet ----------

    [Theory]
    [InlineData("city")]
    [InlineData("City")]
    [InlineData("CITY")]
    [InlineData("  city  ")]
    public void TryGet_NormalisesTheKey(string key)
    {
        Assert.True(ResourceCatalog.TryGet(key, out var descriptor));
        Assert.Equal("city", descriptor.Key);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData("notaresource")]
    public void TryGet_RejectsAnythingElse(string? key)
    {
        Assert.False(ResourceCatalog.TryGet(key, out _));
    }

    // ---------- Catalog shape ----------

    [Fact]
    public void Keys_AndAll_DescribeTheSameResources()
    {
        Assert.Equal(
            ResourceCatalog.Keys.OrderBy(k => k),
            ResourceCatalog.All.Select(d => d.Key).OrderBy(k => k));
    }

    [Fact]
    public void EveryResource_HasARouteAndADescription()
    {
        Assert.All(ResourceCatalog.All, descriptor =>
        {
            Assert.False(string.IsNullOrWhiteSpace(descriptor.Route), $"'{descriptor.Key}' has no route.");
            Assert.False(string.IsNullOrWhiteSpace(descriptor.Description), $"'{descriptor.Key}' has no description.");
            Assert.StartsWith("api/v1/", descriptor.Route);
        });
    }

    [Fact]
    public void Operations_ListByName_OnlyForResourcesThatHaveAName()
    {
        Assert.Contains("byName", Get("city").Operations);
        Assert.DoesNotContain("byName", Get("constitutionarticle").Operations);
        Assert.DoesNotContain("byName", Get("postalcode").Operations);
    }

    [Fact]
    public void Operations_AlwaysCoverTheGenericTools()
    {
        Assert.All(ResourceCatalog.All, descriptor =>
        {
            Assert.Contains("list", descriptor.Operations);
            Assert.Contains("byId", descriptor.Operations);
            Assert.Contains("search", descriptor.Operations);
            Assert.Contains("paged", descriptor.Operations);
        });
    }

    [Fact]
    public void SupportsByName_MatchesWhetherTheModelHasANameProperty()
    {
        Assert.True(Get("volcano").SupportsByName);
        Assert.False(Get("constitutionarticle").SupportsByName);
    }

    // ---------- Descriptor delegates ----------

    [Fact]
    public void List_WithNoSortArguments_ReturnsEveryRow()
    {
        using var db = NewSeededContext();

        var (data, validSort) = Get("volcano").List(db, null, null);

        Assert.True(validSort);
        Assert.Equal(VolcanoSeed.TotalRows, Assert.IsAssignableFrom<System.Collections.ICollection>(data).Count);
    }

    [Fact]
    public void List_WithAnUnknownSortField_IsRejected()
    {
        using var db = NewSeededContext();

        var (data, validSort) = Get("volcano").List(db, "notAProperty", "asc");

        Assert.False(validSort);
        Assert.Null(data);
    }

    [Fact]
    public void GetById_ReturnsNullForAnUnknownId()
    {
        using var db = NewSeededContext();

        Assert.Null(Get("volcano").GetById(db, 9999));
        Assert.NotNull(Get("volcano").GetById(db, 1));
    }

    [Fact]
    public void GetByName_MatchesCaseInsensitively()
    {
        using var db = NewSeededContext();

        var matches = Assert.IsAssignableFrom<System.Collections.ICollection>(Get("city").GetByName(db, "medellín"));

        Assert.Equal(1, matches.Count);
    }

    /// <summary>A blank name matches every row, since every Name contains the empty string.</summary>
    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData(null)]
    public void GetByName_WithABlankName_MatchesEverything(string? name)
    {
        using var db = NewSeededContext();

        var matches = Assert.IsAssignableFrom<System.Collections.ICollection>(Get("volcano").GetByName(db, name!));

        Assert.Equal(VolcanoSeed.TotalRows, matches.Count);
    }

    [Fact]
    public void Search_IsAccentInsensitive()
    {
        using var db = NewSeededContext();

        var matches = Assert.IsAssignableFrom<System.Collections.ICollection>(Get("city").Search(db, "MEDELLIN"));

        Assert.Equal(1, matches.Count);
    }

    [Theory]
    [InlineData("")]
    [InlineData(null)]
    public void Search_WithABlankKeyword_MatchesNothing(string? keyword)
    {
        using var db = NewSeededContext();

        var matches = Assert.IsAssignableFrom<System.Collections.ICollection>(Get("volcano").Search(db, keyword!));

        Assert.Equal(0, matches.Count);
    }

    [Fact]
    public void Paged_ReportsTheTotalIndependentlyOfThePageSize()
    {
        using var db = NewSeededContext();

        var (data, validSort) = Get("volcano").Paged(db, 1, 1, null, null);

        Assert.True(validSort);
        var response = Assert.IsType<api.Utils.PaginationResponseModel<api.Models.Volcano>>(data);
        Assert.Equal(VolcanoSeed.TotalRows, response.TotalRecords);
        Assert.Single(response.Data);
    }

    [Fact]
    public void Paged_WithAnUnknownSortField_IsRejected()
    {
        using var db = NewSeededContext();

        var (data, validSort) = Get("volcano").Paged(db, 1, 10, "notAProperty", "asc");

        Assert.False(validSort);
        Assert.Null(data);
    }

    [Fact]
    public void Paged_PastTheLastPage_ReturnsAnEmptyPageNotAnError()
    {
        using var db = NewSeededContext();

        var (data, validSort) = Get("volcano").Paged(db, 999, 10, null, null);

        Assert.True(validSort);
        var response = Assert.IsType<api.Utils.PaginationResponseModel<api.Models.Volcano>>(data);
        Assert.Empty(response.Data);
        Assert.Equal(VolcanoSeed.TotalRows, response.TotalRecords);
    }
}
