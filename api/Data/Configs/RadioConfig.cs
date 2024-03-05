using api.Models;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace api.Data.Configs
{
    public class RadioConfig : IEntityTypeConfiguration<Radio>
    {
        public void Configure(EntityTypeBuilder<Radio> radio)
        {
            radio.ToTable("Radio");
            radio.HasKey(p => p.Id);
            radio.Property(p => p.Id).ValueGeneratedOnAdd();
            radio.Property(p => p.Name).IsRequired().HasMaxLength(150);
            radio.Property(p => p.Frequency).IsRequired().HasMaxLength(10000);
            radio.Property(p => p.Url).IsRequired(false);
            radio.Property(p => p.Streamers).IsRequired(false);
            radio.Property(p => p.CityId);
            radio.HasOne(p => p.City).WithMany(p => p.Radios).HasForeignKey(p => p.CityId);

        }
    }
}
