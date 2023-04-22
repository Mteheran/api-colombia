using api.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
public class MapConfig : IEntityTypeConfiguration<Map>
{
    public void Configure(EntityTypeBuilder<Map> map)
    {
        map.ToTable("Map");
        map.HasKey(p => p.Id);
        map.Property(p => p.Name).IsRequired().HasMaxLength(300);
        map.Property(p => p.Description).IsRequired(false);
        map.Property(p => p.DepartamentId).IsRequired(false);
        map.Property(p => p.UrlImages).IsRequired(true);
        map.Property(p => p.UrlSource).IsRequired(false);
        map.HasOne(p => p.Departament).WithMany(p => p.Maps).HasForeignKey(p => p.DepartamentId);
    }
}
