using System.Reflection;
using api.Models;
using api.Utils;
using Microsoft.EntityFrameworkCore;

namespace api.Mcp;

/// <summary>
/// Describes a single API Colombia resource and how to query it. Acts as the single
/// source of truth shared by the generic MCP data tools and the guidance tools, so
/// adding a new resource here automatically exposes it through every tool.
/// </summary>
public sealed class ResourceDescriptor
{
    public required string Key { get; init; }
    public required string Route { get; init; }
    public required string Description { get; init; }
    public required bool SupportsByName { get; init; }

    public required Func<DBContext, string?, string?, (object? Data, bool ValidSort)> List { get; init; }
    public required Func<DBContext, int, object?> GetById { get; init; }
    public required Func<DBContext, string, object> GetByName { get; init; }
    public required Func<DBContext, string, object> Search { get; init; }
    public required Func<DBContext, int, int, string?, string?, (object? Data, bool ValidSort)> Paged { get; init; }

    /// <summary>Read-only list of the operations this resource supports, for the guidance tools.</summary>
    public IReadOnlyList<string> Operations
    {
        get
        {
            var ops = new List<string> { "list", "byId", "search", "paged" };
            if (SupportsByName) ops.Insert(2, "byName");
            return ops;
        }
    }
}

/// <summary>
/// Central registry of every queryable resource exposed by the MCP server. Reuses the
/// existing <see cref="Functions.ApplySorting{T}"/> and
/// <see cref="Functions.FilterObjectListPropertiesByKeyword{T}"/> helpers so the MCP
/// surface behaves exactly like the REST endpoints without duplicating their logic.
/// </summary>
public static class ResourceCatalog
{
    private static readonly IReadOnlyDictionary<string, ResourceDescriptor> _resources = Build();

    public static IEnumerable<ResourceDescriptor> All => _resources.Values;

    public static bool TryGet(string? key, out ResourceDescriptor descriptor)
        => _resources.TryGetValue((key ?? string.Empty).Trim().ToLowerInvariant(), out descriptor!);

    public static IEnumerable<string> Keys => _resources.Keys;

    private static IReadOnlyDictionary<string, ResourceDescriptor> Build()
    {
        var dict = new Dictionary<string, ResourceDescriptor>(StringComparer.OrdinalIgnoreCase);

        void Add<T>(string key, string route, string description, Func<DBContext, IQueryable<T>> query)
            where T : class
        {
            var hasName = typeof(T).GetProperty("Name", BindingFlags.Public | BindingFlags.Instance) is not null;

            dict[key] = new ResourceDescriptor
            {
                Key = key,
                Route = route,
                Description = description,
                SupportsByName = hasName,
                List = (db, sortBy, sortDirection) =>
                {
                    var (q, ok) = Functions.ApplySorting(query(db), sortBy ?? string.Empty, sortDirection ?? string.Empty);
                    return ok ? (q.ToList(), true) : ((object?)null, false);
                },
                GetById = (db, id) => query(db).FirstOrDefault(e => EF.Property<int>(e, "Id") == id),
                GetByName = (db, name) =>
                {
                    var search = (name ?? string.Empty).Trim();
                    return query(db).ToList().Where(e => NameMatches(e, search)).ToList();
                },
                Search = (db, keyword) =>
                {
                    var wellFormed = (keyword ?? string.Empty).Trim().ToUpper().Normalize();
                    return Functions.FilterObjectListPropertiesByKeyword(query(db).ToList(), wellFormed);
                },
                Paged = (db, page, pageSize, sortBy, sortDirection) =>
                {
                    var (q, ok) = Functions.ApplySorting(query(db), sortBy ?? string.Empty, sortDirection ?? string.Empty);
                    if (!ok) return ((object?)null, false);

                    var total = q.Count();
                    var data = q.Skip((page - 1) * pageSize).Take(pageSize).ToList();
                    var response = new PaginationResponseModel<T>
                    {
                        Page = page,
                        PageSize = pageSize,
                        TotalRecords = total,
                        Data = data
                    };
                    return (response, true);
                }
            };
        }

        Add<Country>("country", $"{Util.API_ROUTE}{Util.API_VERSION}{Util.COUNTRY_ROUTE}", "General information about Colombia (time zone, languages, currency, etc.).", db => db.Countries);
        Add<Department>("department", $"{Util.API_ROUTE}{Util.API_VERSION}{Util.DEPARTMENT_ROUTE}", "Departments (states) of Colombia, including their capital city.", db => db.Departments.Include(d => d.CityCapital));
        Add<City>("city", $"{Util.API_ROUTE}{Util.API_VERSION}{Util.CITY_ROUTE}", "Cities of Colombia, including the department they belong to.", db => db.Cities.Include(c => c.Department));
        Add<President>("president", $"{Util.API_ROUTE}{Util.API_VERSION}{Util.PRESIDENT_ROUTE}", "Presidents of Colombia and their governing periods.", db => db.Presidents);
        Add<TouristAttraction>("touristattraction", $"{Util.API_ROUTE}{Util.API_VERSION}{Util.TOURISTIC_ROUTE}", "Tourist attractions across Colombia, including the city they are in.", db => db.TouristAttractions.Include(t => t.City));
        Add<Region>("region", $"{Util.API_ROUTE}{Util.API_VERSION}{Util.REGION}", "Natural regions of Colombia.", db => db.Regions);
        Add<CategoryNaturalArea>("categorynaturalarea", $"{Util.API_ROUTE}{Util.API_VERSION}{Util.CATEGORY_NATURAL_AREA}", "Categories used to classify natural areas.", db => db.CategoryNaturalAreas);
        Add<NaturalArea>("naturalarea", $"{Util.API_ROUTE}{Util.API_VERSION}{Util.NATURAL_AREA}", "Protected natural areas of Colombia.", db => db.NaturalAreas);
        Add<Map>("map", $"{Util.API_ROUTE}{Util.API_VERSION}{Util.MAP_ROUTE}", "Maps of Colombia.", db => db.Maps);
        Add<InvasiveSpecie>("invasivespecie", $"{Util.API_ROUTE}{Util.API_VERSION}{Util.INVASIVE_SPECIE_ROUTE}", "Invasive species found in Colombia.", db => db.InvasiveSpecies);
        Add<NativeCommunity>("nativecommunity", $"{Util.API_ROUTE}{Util.API_VERSION}{Util.NATIVE_COMMUNITY_ROUTE}", "Native communities of Colombia.", db => db.NativeCommunities);
        Add<IndigenousReservation>("indigenousreservation", $"{Util.API_ROUTE}{Util.API_VERSION}{Util.INDIGENOUS_RESERVATION_ROUTE}", "Indigenous reservations of Colombia.", db => db.IndigenousReservations);
        Add<Airport>("airport", $"{Util.API_ROUTE}{Util.API_VERSION}{Util.AIRPORT}", "Airports located in Colombia.", db => db.Airports);
        Add<ConstitutionArticle>("constitutionarticle", $"{Util.API_ROUTE}{Util.API_VERSION}{Util.CONSTITUTION_ARTICLE}", "Articles of the Colombian constitution.", db => db.ConstitutionArticles);
        Add<Radio>("radio", $"{Util.API_ROUTE}{Util.API_VERSION}{Util.RADIO}", "Radio stations of Colombia.", db => db.Radios);
        Add<Holiday>("holiday", $"{Util.API_ROUTE}{Util.API_VERSION}{Util.HOLIDAY_ROUTE}", "Public holidays of Colombia.", db => db.Holidays);
        Add<TypicalDish>("typicaldish", $"{Util.API_ROUTE}{Util.API_VERSION}{Util.TYPICAL_DISH_ROUTE}", "Typical dishes of Colombian gastronomy.", db => db.TypicalDishes);
        Add<TraditionalFairAndFestival>("traditionalfairandfestival", $"{Util.API_ROUTE}{Util.API_VERSION}{Util.TRADITIONAL_FAIR_AND_FESTIVAL_ROUTE}", "Traditional fairs and festivals of Colombia.", db => db.TraditionalFairAndFestival);
        Add<IntangibleHeritage>("intangibleheritage", $"{Util.API_ROUTE}{Util.API_VERSION}{Util.INTANGIBLE_HERITAGE_ROUTE}", "Intangible cultural heritage of Colombia.", db => db.IntangibleHeritages);
        Add<HeritageCity>("heritagecity", $"{Util.API_ROUTE}{Util.API_VERSION}{Util.HERITAGE_CITY_ROUTE}", "Heritage cities of Colombia.", db => db.HeritageCities);
        Add<PostalCode>("postalcode", $"{Util.API_ROUTE}{Util.API_VERSION}{Util.POSTAL_CODE_ROUTE}", "Postal codes of Colombia.", db => db.PostalCodes);
        Add<UrbanCenter>("urbancenter", $"{Util.API_ROUTE}{Util.API_VERSION}{Util.URBAN_CENTER_ROUTE}", "Urban centers of Colombia (municipal heads and populated centers).", db => db.UrbanCenters);

        return dict;
    }

    private static bool NameMatches(object entity, string search)
    {
        var value = entity.GetType()
            .GetProperty("Name", BindingFlags.Public | BindingFlags.Instance)?
            .GetValue(entity) as string;

        return value is not null
            && value.Contains(search, StringComparison.OrdinalIgnoreCase);
    }
}
