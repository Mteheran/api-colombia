using api.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class IntangibleHeritageConfig : IEntityTypeConfiguration<IntangibleHeritage>
{
    public void Configure(EntityTypeBuilder<IntangibleHeritage> intangibleHeritage)
    {
        intangibleHeritage.ToTable("IntangibleHeritage");
        intangibleHeritage.HasKey(t => t.Id);
        intangibleHeritage.Property(t => t.Id).ValueGeneratedOnAdd();
        intangibleHeritage.Property(t => t.Name).IsRequired().HasMaxLength(200);
        intangibleHeritage.Property(t => t.Scope).IsRequired(false).HasMaxLength(500);
        intangibleHeritage.Property(t => t.InclusionYear).IsRequired(false);
        intangibleHeritage.Property(t => t.DepartmentId).IsRequired(false);
        intangibleHeritage.HasOne(t => t.Department).WithMany().HasForeignKey(t => t.DepartmentId).OnDelete(DeleteBehavior.SetNull);
    }
}
