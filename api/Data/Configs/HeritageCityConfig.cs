using api.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class HeritageCityConfig : IEntityTypeConfiguration<HeritageCity>
{
    public void Configure(EntityTypeBuilder<HeritageCity> heritageCity)
    {
        heritageCity.ToTable("HeritageCity");
        heritageCity.HasKey(t => t.Id);
        heritageCity.Property(t => t.Id).ValueGeneratedOnAdd();
        heritageCity.Property(t => t.Name).IsRequired().HasMaxLength(200);
        heritageCity.Property(t => t.Description).IsRequired().HasMaxLength(2000);
        heritageCity.Property(t => t.Image).IsRequired().HasMaxLength(255);
        heritageCity.Property(t => t.CityId).IsRequired();
        heritageCity.Property(t => t.DepartmentId).IsRequired();
        heritageCity.HasOne(t => t.City).WithMany().HasForeignKey(t => t.CityId).OnDelete(DeleteBehavior.SetNull);
        heritageCity.HasOne(t => t.Department).WithMany().HasForeignKey(t => t.DepartmentId).OnDelete(DeleteBehavior.SetNull);
    }
}
