using api.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
public class TouristAttractionConfig : IEntityTypeConfiguration<TouristAttraction>
{
    public void Configure(EntityTypeBuilder<TouristAttraction> touristAttraction)
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
        touristAttraction.HasOne(p => p.City).WithMany(p => p.TouristAttractions).HasForeignKey(p => p.CityId);
    }
}