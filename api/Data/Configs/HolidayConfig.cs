using api.Models;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace api.Data.Configs
{
    public class HolidayConfig : IEntityTypeConfiguration<Holiday>
    {
        public void Configure(EntityTypeBuilder<Holiday> holiday)
        {
            holiday.ToTable("Holidays"); 
            holiday.HasKey(h => h.Date);   
            holiday.Property(h => h.Name).IsRequired().HasMaxLength(150);    
        }
     }
}
