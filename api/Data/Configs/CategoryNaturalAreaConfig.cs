using api.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class CategoryNaturalAreaConfig : IEntityTypeConfiguration<CategoryNaturalArea>
{
    public void Configure(EntityTypeBuilder<CategoryNaturalArea> category)
    {
        category.ToTable("CategoryNaturalArea");
        category.HasKey(p => p.Id);
        category.Property(p => p.Id).ValueGeneratedOnAdd();
        category.Property(p => p.Name).IsRequired().HasMaxLength(150);
        category.Property(p => p.Description).IsRequired(false);
    }
}