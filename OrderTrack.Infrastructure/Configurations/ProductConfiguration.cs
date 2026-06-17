using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OrderTrack.Domain.Entities;

namespace OrderTrack.Infrastructure.Configurations;

public class ProductConfiguration : IEntityTypeConfiguration<Product>
{
    public void Configure(EntityTypeBuilder<Product> builder)
    {
        builder.ToTable("products", "public");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
            .HasColumnName("id");

        builder.Property(x => x.Name)
            .HasColumnName("name")
            .IsRequired();

        builder.Property(x => x.ProductionHours)
            .HasColumnName("production_hours")
            .HasColumnType("numeric(6,2)")
            .IsRequired();

        builder.Property(x => x.IsAvailable)
            .HasColumnName("is_available")
            .HasDefaultValue(true);

        builder.Property(x => x.StockQuantity)
            .HasColumnName("stock_quantity");

        builder.Property(x => x.IsActive)
            .HasColumnName("is_active")
            .HasDefaultValue(true);

        builder.Property(x => x.IsVariableQuantity)
            .HasColumnName("is_variable_quantity")
            .HasDefaultValue(false);

        builder.Property(x => x.MinimumQuantity)
            .HasColumnName("minimum_quantity");

        builder.Property(x => x.QuantityStep)
            .HasColumnName("quantity_step");

        builder.Property(x => x.HoursPerStep)
            .HasColumnName("hours_per_step")
            .HasColumnType("numeric(6,2)");

        builder.Property(x => x.CreatedAt)
            .HasColumnName("created_at");

        builder.Property(x => x.UpdatedAt)
            .HasColumnName("updated_at");
    }
}