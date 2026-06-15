using api.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace api.Data.Configs
{
    public class UrbanCenterConfig : IEntityTypeConfiguration<UrbanCenter>
    {
        public void Configure(EntityTypeBuilder<UrbanCenter> builder)
        {
            builder.ToTable("UrbanCenter");
            builder.HasKey(p => p.Id);
            builder.Property(p => p.Id).ValueGeneratedOnAdd();
            builder.Property(p => p.CityId).IsRequired();
            builder.Property(p => p.Code).IsRequired().HasMaxLength(20);
            builder.Property(p => p.Name).IsRequired().HasMaxLength(200);
            builder.Property(p => p.Type).IsRequired().HasMaxLength(50);
            builder.Property(p => p.Longitude).IsRequired();
            builder.Property(p => p.Latitude).IsRequired();

            builder.HasOne(p => p.City)
                .WithMany(p => p.UrbanCenters)
                .HasForeignKey(p => p.CityId);
        }
    }
}
