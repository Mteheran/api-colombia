using Microsoft.EntityFrameworkCore;
using api.Models;

namespace api;
public class DBContext : DbContext
{
    public DbSet<Country> Countries { get; set; }
    public DbSet<Departament> Deparments { get; set; }
    public DbSet<President> Presidents { get; set; }
    public DbSet<City> Cities { get; set; }
    public DbSet<TouristAttraction> TouristAttractions { get; set; }

    public DBContext(DbContextOptions<DBContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<Country>(country =>
        {
            country.ToTable("Country");
            country.HasKey(p => p.Id);
            country.Property(p => p.Name).IsRequired().HasMaxLength(150);
            country.Property(p => p.Description).IsRequired(false);
            country.Property(p => p.StateCapital);
            country.Property(p => p.Surface);
            country.Property(p => p.Population);
            country.Property(p => p.Languages);
            country.Property(p => p.TimeZone).HasMaxLength(150);
            country.Property(p => p.Currency).HasMaxLength(10);
            country.Property(p => p.ISOCode).HasMaxLength(10);
            country.Property(p => p.InternetDomain);
            country.Property(p => p.PhonePrefix).HasMaxLength(5);
            country.Property(p => p.RadioPrefix).HasMaxLength(5);;
            country.Property(p => p.AircraftPrefix).HasMaxLength(5);
        });

        builder.Entity<Departament>(deparment =>
        {
            deparment.ToTable("Deparment");
            deparment.HasKey(p => p.Id);
            deparment.Property(p => p.Id).ValueGeneratedOnAdd();
            deparment.Property(p => p.Name).IsRequired().HasMaxLength(150);
            deparment.Property(p => p.Description).IsRequired(false);
            deparment.Property(p => p.CityCapitalId);
            deparment.Property(p => p.Municipalities);
            deparment.Property(p => p.Population);
            deparment.Property(p => p.Surface);
            deparment.Property(p => p.PhonePrefix).HasMaxLength(5);
            deparment.Property(p => p.CountryId);
            deparment.HasOne(p=> p.Country).WithMany(p=> p.Departaments).HasForeignKey(p=> p.CountryId);

        });

        builder.Entity<City>(city =>
        {
            city.ToTable("City");
            city.HasKey(p => p.Id);
            city.Property(p => p.Id).ValueGeneratedOnAdd();
            city.Property(p => p.Name).IsRequired().HasMaxLength(150);
            city.Property(p => p.Description).IsRequired(false);
            city.Property(p => p.Population);
            city.Property(p => p.Surface);
            city.Property(p => p.PostalCode).HasMaxLength(10); ;
            city.Property(p => p.PhonePrefix).HasMaxLength(5);
            city.Property(p => p.DepartamentId);
            city.HasOne(p=> p.Departament).WithMany(p=> p.Cities).HasForeignKey(p=> p.DepartamentId);
        });

        builder.Entity<President>(president =>
        {
            president.ToTable("President");
            president.HasKey(p => p.Id);
            president.Property(p => p.Id).ValueGeneratedOnAdd();
            president.Property(p => p.Name).IsRequired().HasMaxLength(150);
            president.Property(p => p.LastName).IsRequired().HasMaxLength(150);
            president.Property(p => p.StartPeriodDate).IsRequired();
            president.Property(p => p.EndPeriodDate).IsRequired();
            president.Property(p => p.PoliticalParty).IsRequired();
            president.Property(p => p.Description).IsRequired(false);
            president.Property(p => p.Image).IsRequired(false);
            president.Property(p => p.CityId);
            president.HasOne(p=> p.City).WithMany(p=> p.Presidents).HasForeignKey(p=> p.CityId);
        });

        builder.Entity<TouristAttraction>(touristAttraction =>
        {
            touristAttraction.ToTable("TouristAttraction");
            touristAttraction.HasKey(p => p.Id);
            touristAttraction.Property(p => p.Id).ValueGeneratedOnAdd();
            touristAttraction.Property(p => p.Name).IsRequired().HasMaxLength(150);
            touristAttraction.Property(p => p.Description).IsRequired(false);
            touristAttraction.Property(p => p.Images).IsRequired(false);
            touristAttraction.Property(p => p.Latitude).IsRequired(false);
            touristAttraction.Property(p => p.Longitude).IsRequired(false);
            touristAttraction.Property(p => p.CityId);
            touristAttraction.HasOne(p=> p.City).WithMany(p=> p.TouristAttractions).HasForeignKey(p=> p.CityId);

        });

    }

}