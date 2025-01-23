using api.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders; 

public class TypicalDishConfig : IEntityTypeConfiguration<TypicalDish>
{
    public void Configure(EntityTypeBuilder<TypicalDish> typicalDish)
    {
        typicalDish.ToTable("TypicalDish");
        typicalDish.HasKey(t => t.Id); 
        typicalDish.Property(t => t.Id).ValueGeneratedOnAdd();  
        typicalDish.Property(t => t.Name).IsRequired().HasMaxLength(200);   
        typicalDish.Property(t => t.Description).IsRequired(true).HasMaxLength(1000);   
        typicalDish.Property(t => t.Ingredients).IsRequired(true).HasMaxLength(1000); 
        typicalDish.Property(t => t.ImageUrl).IsRequired(true).HasMaxLength(255);  
        typicalDish.Property(t => t.DepartmentId).IsRequired(true);  
        typicalDish.HasOne(t => t.Department).WithMany().HasForeignKey(t => t.DepartmentId).OnDelete(DeleteBehavior.SetNull);  
    }
}
