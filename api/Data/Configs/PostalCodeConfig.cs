using api.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class PostalCodeConfig : IEntityTypeConfiguration<PostalCode>
{
    public void Configure(EntityTypeBuilder<PostalCode> postalCode)
    {
        postalCode.ToTable("PostalCode");
        postalCode.HasKey(t => t.Id);
        postalCode.Property(t => t.Id).ValueGeneratedOnAdd();
        postalCode.Property(t => t.NoId).IsRequired();
        postalCode.Property(t => t.CityId).IsRequired();
        postalCode.Property(t => t.PostalZone).IsRequired().HasMaxLength(255);
        postalCode.Property(t => t.Code).IsRequired().HasMaxLength(255);
        postalCode.Property(t => t.NorthLimit).IsRequired().HasMaxLength(2000);
        postalCode.Property(t => t.SouthLimit).IsRequired().HasMaxLength(2000);
        postalCode.Property(t => t.EastLimit).IsRequired().HasMaxLength(2000);
        postalCode.Property(t => t.WestLimit).IsRequired().HasMaxLength(2000);
        postalCode.Property(t => t.Type).IsRequired().HasMaxLength(255);
        postalCode.Property(t => t.NeighborhoodsContainedInPostalCode).IsRequired().HasMaxLength(2000);
        postalCode.Property(t => t.RuralAreasContainedInPostalCode).IsRequired().HasMaxLength(2000);
        postalCode.HasOne(t => t.City).WithMany().HasForeignKey(t => t.CityId).OnDelete(DeleteBehavior.SetNull);
    }
}