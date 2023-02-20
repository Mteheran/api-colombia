using api.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
public class DepartamentConfig : IEntityTypeConfiguration<Department>
{
    public void Configure(EntityTypeBuilder<Department> deparment)
    {
        deparment.ToTable("Department");
        deparment.HasKey(p => p.Id);
        deparment.Property(p => p.Id).ValueGeneratedOnAdd();
        deparment.Property(p => p.Name).IsRequired().HasMaxLength(150);
        deparment.Property(p => p.Description).IsRequired(false);
        deparment.Property(p => p.CityCapitalId).IsRequired(false);
        deparment.Property(p => p.Municipalities);
        deparment.Property(p => p.Population);
        deparment.Property(p => p.Surface);
        deparment.Property(p => p.PhonePrefix).HasMaxLength(5);
        deparment.Property(p => p.CountryId);
        deparment.Property(p => p.RegionId).IsRequired(false);
        deparment.HasOne(p => p.Country).WithMany(p => p.Departments).HasForeignKey(p => p.CountryId);
        deparment.HasOne(p => p.Region).WithMany(p => p.Departments).HasForeignKey(p => p.RegionId);
    }
}