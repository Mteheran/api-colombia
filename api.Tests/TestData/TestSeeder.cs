using api.Models;
using Microsoft.EntityFrameworkCore;

namespace api.Tests.TestData;

/// <summary>
/// Fills a test database. Runs exactly once per database, so there are no `if (!db.X.Any())`
/// guards: every row is added unconditionally and explicitly.
///
/// <see cref="ExpectedRowCounts"/> is the single source of truth for how many rows each table
/// holds; <c>SeedIntegrityTests</c> asserts the database matches it, which is what catches a
/// seeder that silently drops (or smuggles in) rows via navigation properties.
/// </summary>
internal static class TestSeeder
{
    public static void Seed(DBContext db)
    {
        var core = new CoreGraph();
        db.AddRange(core.All);

        var categories = CategoryNaturalAreaSeed.Rows();

        db.AddRange(categories);
        db.AddRange(NaturalAreaSeed.Rows(core, categories));
        db.AddRange(TouristAttractionSeed.Rows(core));
        db.AddRange(ConstitutionArticleSeed.Rows());
        db.AddRange(AirportSeed.Rows(core));
        db.AddRange(CountrySeed.Rows(core));
        db.AddRange(MapSeed.Rows());
        db.AddRange(IndigenousReservationSeed.Rows(core));
        db.AddRange(InvasiveSpecieSeed.Rows());
        db.AddRange(NativeCommunitySeed.Rows());
        db.AddRange(PresidentSeed.Rows(core));
        db.AddRange(RadioSeed.Rows(core));
        db.AddRange(TraditionalFairAndFestivalSeed.Rows(core));
        db.AddRange(TypicalDishSeed.Rows(core));
        db.AddRange(IntangibleHeritageSeed.Rows(core));
        db.AddRange(HeritageCitySeed.Rows(core));
        db.AddRange(PostalCodeSeed.Rows(core));
        db.AddRange(UrbanCenterSeed.Rows(core));
        db.AddRange(HigherEducationInstitutionSeed.Rows(core));
        db.AddRange(TelevisionChannelSeed.Rows(core));
        db.AddRange(VolcanoSeed.Rows(core));

        db.SaveChanges();
    }

    /// <summary>Every seeded table and the number of rows it must contain after <see cref="Seed"/>.</summary>
    public static IReadOnlyDictionary<string, (int Expected, Func<DBContext, int> Actual)> ExpectedRowCounts { get; } =
        new Dictionary<string, (int, Func<DBContext, int>)>
        {
            [nameof(DBContext.Regions)]                    = (CoreGraph.RegionRows, db => db.Regions.Count()),
            [nameof(DBContext.Departments)]                = (CoreGraph.DepartmentRows, db => db.Departments.Count()),
            [nameof(DBContext.Cities)]                     = (CoreGraph.CityRows, db => db.Cities.Count()),
            [nameof(DBContext.CategoryNaturalAreas)]       = (CategoryNaturalAreaSeed.TotalRows, db => db.CategoryNaturalAreas.Count()),
            [nameof(DBContext.NaturalAreas)]               = (NaturalAreaSeed.TotalRows, db => db.NaturalAreas.Count()),
            [nameof(DBContext.TouristAttractions)]         = (TouristAttractionSeed.TotalRows, db => db.TouristAttractions.Count()),
            [nameof(DBContext.ConstitutionArticles)]       = (ConstitutionArticleSeed.TotalRows, db => db.ConstitutionArticles.Count()),
            [nameof(DBContext.Airports)]                   = (AirportSeed.TotalRows, db => db.Airports.Count()),
            [nameof(DBContext.Countries)]                  = (CountrySeed.TotalRows, db => db.Countries.Count()),
            [nameof(DBContext.Maps)]                       = (MapSeed.TotalRows, db => db.Maps.Count()),
            [nameof(DBContext.IndigenousReservations)]     = (IndigenousReservationSeed.TotalRows, db => db.IndigenousReservations.Count()),
            [nameof(DBContext.InvasiveSpecies)]            = (InvasiveSpecieSeed.TotalRows, db => db.InvasiveSpecies.Count()),
            [nameof(DBContext.NativeCommunities)]          = (NativeCommunitySeed.TotalRows, db => db.NativeCommunities.Count()),
            [nameof(DBContext.Presidents)]                 = (PresidentSeed.TotalRows, db => db.Presidents.Count()),
            [nameof(DBContext.Radios)]                     = (RadioSeed.TotalRows, db => db.Radios.Count()),
            [nameof(DBContext.TraditionalFairAndFestival)] = (TraditionalFairAndFestivalSeed.TotalRows, db => db.TraditionalFairAndFestival.Count()),
            [nameof(DBContext.TypicalDishes)]              = (TypicalDishSeed.TotalRows, db => db.TypicalDishes.Count()),
            [nameof(DBContext.IntangibleHeritages)]        = (IntangibleHeritageSeed.TotalRows, db => db.IntangibleHeritages.Count()),
            [nameof(DBContext.HeritageCities)]             = (HeritageCitySeed.TotalRows, db => db.HeritageCities.Count()),
            [nameof(DBContext.PostalCodes)]                = (PostalCodeSeed.TotalRows, db => db.PostalCodes.Count()),
            [nameof(DBContext.UrbanCenters)]               = (UrbanCenterSeed.TotalRows, db => db.UrbanCenters.Count()),
            [nameof(DBContext.HigherEducationInstitutions)]= (HigherEducationInstitutionSeed.TotalRows, db => db.HigherEducationInstitutions.Count()),
            [nameof(DBContext.TelevisionChannels)]         = (TelevisionChannelSeed.TotalRows, db => db.TelevisionChannels.Count()),
            [nameof(DBContext.Volcanoes)]                  = (VolcanoSeed.TotalRows, db => db.Volcanoes.Count()),
        };
}
