using api.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
public class CountryConfig : IEntityTypeConfiguration<Country>
{
    public void Configure(EntityTypeBuilder<Country> country)
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
        country.Property(p => p.Currency).HasMaxLength(15);
        country.Property(p => p.CurrencyCode).HasMaxLength(10);
        country.Property(p => p.CurrencySymbol).HasMaxLength(2);
        country.Property(p => p.ISOCode).HasMaxLength(10);
        country.Property(p => p.InternetDomain);
        country.Property(p => p.PhonePrefix).HasMaxLength(5);
        country.Property(p => p.RadioPrefix).HasMaxLength(5); ;
        country.Property(p => p.AircraftPrefix).HasMaxLength(5);
        country.Property(p => p.SubRegion).HasMaxLength(20);
        country.Property(p => p.Region).HasMaxLength(20);
        country.Property(p => p.Borders);
        country.Property(p => p.Flags);
    }
}
