using api.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class RegionConfig : IEntityTypeConfiguration<Region>
{
    public void Configure(EntityTypeBuilder<Region> city)
    {
        city.ToTable("Region");
        city.HasKey(p => p.Id);
        city.Property(p => p.Id).ValueGeneratedOnAdd();
        city.Property(p => p.Name).IsRequired().HasMaxLength(150);
        city.Property(p => p.Description).IsRequired(false);
    }
}

