using api.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class ParamoConfig : IEntityTypeConfiguration<Paramo>
{
    public void Configure(EntityTypeBuilder<Paramo> paramo)
    {
        paramo.ToTable("Paramo");
        paramo.HasKey(p => p.Id);
        paramo.Property(p => p.Id).ValueGeneratedOnAdd();
        paramo.Property(p => p.Name).IsRequired().HasMaxLength(150);
        paramo.Property(p => p.Description).IsRequired(false);
        paramo.Property(p => p.Surface);
        paramo.Property(p => p.CityId);
        paramo.HasOne(p => p.City).WithMany(p => p.Paramos).HasForeignKey(p => p.CityId);
    }
}