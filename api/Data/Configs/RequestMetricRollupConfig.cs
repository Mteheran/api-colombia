using api.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace api.Data.Configs
{
    public class RequestMetricRollupConfig : IEntityTypeConfiguration<RequestMetricRollup>
    {
        public void Configure(EntityTypeBuilder<RequestMetricRollup> rollup)
        {
            rollup.ToTable("RequestMetricRollup");
            rollup.HasKey(p => p.Id);
            rollup.Property(p => p.Id).ValueGeneratedOnAdd();
            rollup.Property(p => p.HourBucketUtc).IsRequired();
            rollup.Property(p => p.RequestCount);
            rollup.Property(p => p.TotalResponseBytes);
            rollup.Property(p => p.MaxResponseBytes);
            rollup.Property(p => p.LargestRequestPath).HasMaxLength(2048).IsRequired(false);
            rollup.HasIndex(p => p.HourBucketUtc).IsUnique();
        }
    }
}
