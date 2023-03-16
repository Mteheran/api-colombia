using api.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class NaturalAreaConfig : IEntityTypeConfiguration<NaturalArea>
{
    public void Configure(EntityTypeBuilder<NaturalArea> naturalArea)
    {
        naturalArea.ToTable("NaturalArea");
        naturalArea.HasKey(p => p.Id);
        naturalArea.Property(p => p.Id).ValueGeneratedOnAdd();
        naturalArea.Property(p => p.AreaGroupId).IsRequired(false);
        naturalArea.Property(p => p.DepartmentId).IsRequired(false);
        naturalArea.Property(p => p.Name).IsRequired().HasMaxLength(150);
        naturalArea.Property(p => p.DaneCode).IsRequired(false);
        naturalArea.Property(p => p.LandArea).IsRequired(false);
        naturalArea.Property(p => p.MaritimeArea).IsRequired(false);
        naturalArea.HasOne(p => p.Department).WithMany(p => p.NaturalAreas).HasForeignKey(p => p.DepartmentId);
        naturalArea.HasOne(p => p.CategoryNaturalArea).WithMany(p => p.NaturalAreas).HasForeignKey(p => p.CategoryNaturalAreaId);
    }
}