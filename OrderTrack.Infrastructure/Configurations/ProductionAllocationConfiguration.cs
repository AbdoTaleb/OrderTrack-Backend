using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OrderTrack.Domain.Entities;

namespace OrderTrack.Infrastructure.Configurations;

public class ProductionAllocationConfiguration : IEntityTypeConfiguration<ProductionAllocation>
{
    public void Configure(EntityTypeBuilder<ProductionAllocation> builder)
    {
        builder.ToTable("production_allocations", "public");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id).HasColumnName("id");

        builder.Property(x => x.OrderId)
            .HasColumnName("order_id")
            .IsRequired();

        builder.Property(x => x.Date)
            .HasColumnName("date")
            .IsRequired();

        builder.Property(x => x.AllocatedHours)
            .HasColumnName("allocated_hours")
            .HasColumnType("numeric(8,2)")
            .IsRequired();

        builder.Property(x => x.CreatedAt)
            .HasColumnName("created_at");

        builder.HasOne(x => x.Order)
            .WithMany(x => x.ProductionAllocations)
            .HasForeignKey(x => x.OrderId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(x => x.Date);
        builder.HasIndex(x => x.OrderId);
    }
}