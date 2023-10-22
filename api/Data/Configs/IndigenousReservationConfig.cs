using api.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
public class IndigenousReservationConfig : IEntityTypeConfiguration<IndigenousReservation>
{
    public void Configure(EntityTypeBuilder<IndigenousReservation> deparment)
    {
        deparment.ToTable("IndigenousReservation");
        deparment.HasKey(p => p.Id);
        deparment.Property(p => p.Id).ValueGeneratedOnAdd();
        deparment.Property(p => p.Name).IsRequired().HasMaxLength(150);
        deparment.Property(p => p.Code).IsRequired(false);
        deparment.Property(p => p.ProcedureType).IsRequired(false);
        deparment.Property(p => p.AdministrativeAct).IsRequired(false);
        deparment.Property(p => p.AdministrativeActType).IsRequired(false);
        deparment.Property(p => p.AdministrativeActNumber).IsRequired(false);
        deparment.Property(p => p.AdministrativeActDate).IsRequired(false);
        deparment.Property(p => p.NativeCommunityId).IsRequired(false);
        deparment.Property(p => p.DeparmentId).IsRequired(false);
        deparment.Property(p => p.CityId).IsRequired(false);

        deparment.HasOne(p => p.NativeCommunity).WithMany(p => p.IndigenousReservations).HasForeignKey(p => p.NativeCommunityId);
        deparment.HasOne(p => p.Department).WithMany(p => p.IndigenousReservations).HasForeignKey(p => p.DeparmentId);
        deparment.HasOne(p => p.City).WithMany(p => p.IndigenousReservations).HasForeignKey(p => p.CityId);

    }
}