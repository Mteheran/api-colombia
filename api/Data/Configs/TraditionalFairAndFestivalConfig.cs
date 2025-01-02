using api.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders; 

public class TraditionalFairAndFestivalConfig : IEntityTypeConfiguration<TraditionalFairAndFestival>
{
     public void Configure(EntityTypeBuilder<TraditionalFairAndFestival> TraditionalFairAndFestivalConfig)
    {
        TraditionalFairAndFestivalConfig.ToTable("TraditionalFairAndFestival");
        TraditionalFairAndFestivalConfig.HasKey(t => t.Id); 
        TraditionalFairAndFestivalConfig.Property(t => t.Id).ValueGeneratedOnAdd();  
        TraditionalFairAndFestivalConfig.Property(t => t.Name).IsRequired().HasMaxLength(100);   
        TraditionalFairAndFestivalConfig.Property(t => t.Description).IsRequired(true).HasMaxLength(200);   
        TraditionalFairAndFestivalConfig.Property(t => t.Month).IsRequired(true).HasMaxLength(50);   
        TraditionalFairAndFestivalConfig.Property(t => t.CityId).IsRequired(true);  
        TraditionalFairAndFestivalConfig.HasOne(t => t.City).WithMany().HasForeignKey(t => t.CityId).OnDelete(DeleteBehavior.SetNull); 
    }
}