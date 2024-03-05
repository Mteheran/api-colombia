using api.Data.Configs;
using api.Models;
using Microsoft.EntityFrameworkCore;

namespace api;
public class DBContext : DbContext
{
    public DbSet<Country> Countries { get; set; }
    public DbSet<Department> Departments { get; set; }
    public DbSet<President> Presidents { get; set; }
    public DbSet<City> Cities { get; set; }
    public DbSet<TouristAttraction> TouristAttractions { get; set; }
    public DbSet<Region> Regions { get; set; }
    public DbSet<CategoryNaturalArea> CategoryNaturalAreas { get; set; }
    public DbSet<NaturalArea> NaturalAreas { get; set; }
    public DbSet<Map> Maps { get; set; }
    public DbSet<InvasiveSpecie> InvasiveSpecies { get; set; }
    public DbSet<NativeCommunity> NativeCommunities { get; set; }
    public DbSet<IndigenousReservation> IndigenousReservations { get; set; }
    public DbSet<Airport> Airports { get; set; }
    public DbSet<ConstitutionArticle> ConstitutionArticles { get; set; }
    public DbSet<Radio> Radios { get; set; }

    public DBContext(DbContextOptions<DBContext> options) : base(options)
    {
        ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.ApplyConfiguration(new CountryConfig());
        builder.ApplyConfiguration(new DepartamentConfig());
        builder.ApplyConfiguration(new CityConfig());
        builder.ApplyConfiguration(new PresidentConfig());
        builder.ApplyConfiguration(new TouristAttractionConfig());
        builder.ApplyConfiguration(new RegionConfig());
        builder.ApplyConfiguration(new CategoryNaturalAreaConfig());
        builder.ApplyConfiguration(new NaturalAreaConfig());
        builder.ApplyConfiguration(new MapConfig());
        builder.ApplyConfiguration(new NativeCommunityConfig());
        builder.ApplyConfiguration(new InvasiveSpecieConfig());
        builder.ApplyConfiguration(new IndigenousReservationConfig());
        builder.ApplyConfiguration(new AirportConfig());
        builder.ApplyConfiguration(new ConstitutionArticleConfig());
        builder.ApplyConfiguration(new RadioConfig());

        base.OnModelCreating(builder);
    }
}
