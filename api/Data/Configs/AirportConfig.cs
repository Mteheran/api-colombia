using api.Models;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

public class AirportConfig : IEntityTypeConfiguration<Airport>
    {
        public void Configure(EntityTypeBuilder<Airport> deparment)
        {
            deparment.ToTable("Airport");
            deparment.HasKey(p => p.Id);
            deparment.Property(p => p.Id).ValueGeneratedOnAdd();
            deparment.Property(p => p.Name).IsRequired().HasMaxLength(150);
            deparment.Property(p => p.IataCode).IsRequired();
            deparment.Property(p => p.OaciCode).IsRequired();
            deparment.Property(p => p.Type).IsRequired();
            deparment.Property(p => p.Latitude).IsRequired();
            deparment.Property(p => p.Longitude).IsRequired();
            deparment.Property(p => p.DeparmentId).IsRequired();
            deparment.Property(p => p.CityId).IsRequired();
            deparment.HasOne(p => p.Department).WithMany(p => p.Airports).HasForeignKey(p => p.DeparmentId);
            deparment.HasOne(p => p.City).WithMany(p => p.Airports).HasForeignKey(p => p.CityId);

        }
}

