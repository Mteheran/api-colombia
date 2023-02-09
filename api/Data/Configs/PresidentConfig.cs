using api.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
public class PresidentConfig : IEntityTypeConfiguration<President>
{
    public void Configure(EntityTypeBuilder<President> president)
    {
        president.ToTable("President");
        president.HasKey(p => p.Id);
        president.Property(p => p.Id).ValueGeneratedOnAdd();
        president.Property(p => p.Name).IsRequired().HasMaxLength(150);
        president.Property(p => p.LastName).IsRequired().HasMaxLength(150);
        president.Property(p => p.StartPeriodDate).IsRequired();
        president.Property(p => p.EndPeriodDate).IsRequired(false);
        president.Property(p => p.PoliticalParty).IsRequired();
        president.Property(p => p.Description).IsRequired(false);
        president.Property(p => p.Image).IsRequired(false);
        president.Property(p => p.CityId);
        president.HasOne(p => p.City).WithMany(p => p.Presidents).HasForeignKey(p => p.CityId);
    }
}
