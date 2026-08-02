using api.Models;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

public class HigherEducationInstitutionConfig : IEntityTypeConfiguration<HigherEducationInstitution>
    {
        public void Configure(EntityTypeBuilder<HigherEducationInstitution> institution)
        {
            institution.ToTable("HigherEducationInstitution");
            institution.HasKey(p => p.Id);
            institution.Property(p => p.Id).ValueGeneratedOnAdd();
            institution.Property(p => p.Code).IsRequired().HasMaxLength(20);
            institution.Property(p => p.Name).IsRequired().HasMaxLength(250);
            institution.Property(p => p.LegalNature).IsRequired().HasMaxLength(50);
            institution.Property(p => p.AcademicCharacter).IsRequired().HasMaxLength(100);
            institution.Property(p => p.CityId).IsRequired();
            institution.Property(p => p.Address).HasMaxLength(250);
            institution.Property(p => p.Phone).HasMaxLength(100);
            institution.Property(p => p.IsHighQualityAccredited).IsRequired();
            institution.Property(p => p.Website).HasMaxLength(250);
            institution.HasOne(p => p.City).WithMany(p => p.HigherEducationInstitutions).HasForeignKey(p => p.CityId);
        }
}
