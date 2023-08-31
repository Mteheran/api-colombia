using api.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
public class NativeCommunityConfig : IEntityTypeConfiguration<NativeCommunity>
{
    public void Configure(EntityTypeBuilder<NativeCommunity> map)
    {
        map.ToTable("NativeCommunity");
        map.HasKey(p => p.Id);
        map.Property(p => p.Name).IsRequired().HasMaxLength(300);
        map.Property(p => p.Description).IsRequired(false);
        map.Property(p => p.Languages).IsRequired(false).HasMaxLength(150);
        map.Property(p => p.Images).IsRequired(false);
    }
}

