using api.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class CityConfig : IEntityTypeConfiguration<City>
{
    public void Configure(EntityTypeBuilder<City> city)
    {
        city.ToTable("City");
        city.HasKey(p => p.Id);
        city.Property(p => p.Id).ValueGeneratedOnAdd();
        city.Property(p => p.Name).IsRequired().HasMaxLength(150);
        city.Property(p => p.Description).IsRequired(false);
        city.Property(p => p.Population);
        city.Property(p => p.Surface);
        city.Property(p => p.PostalCode).HasMaxLength(10);
        city.Property(p => p.DepartamentId);
        city.HasOne(p => p.Departament).WithMany(p => p.Cities).HasForeignKey(p => p.DepartamentId);
    }
}
