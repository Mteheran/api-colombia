using api.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class InvasiveSpecieConfig : IEntityTypeConfiguration<InvasiveSpecie>
{
    public void Configure(EntityTypeBuilder<InvasiveSpecie> invasivespecie)
    {
        invasivespecie.ToTable("InvasiveSpecie");
        invasivespecie.HasKey(p => p.Id);
        invasivespecie.Property(p => p.Id).ValueGeneratedOnAdd();
        invasivespecie.Property(p => p.Name).IsRequired().HasMaxLength(250);
        invasivespecie.Property(p => p.ScientificName).IsRequired(false);
        invasivespecie.Property(p => p.CommonNames).IsRequired(false);
        invasivespecie.Property(p => p.Manage).IsRequired(false);
        invasivespecie.Property(p => p.RiskLevel).IsRequired(true);
        invasivespecie.Property(p => p.UrlImage).IsRequired(false);
    }
}

