using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OrderTrack.Domain.Entities;

namespace OrderTrack.Infrastructure.Configurations;

public class ProductImageConfiguration : IEntityTypeConfiguration<ProductImage>
{
    public void Configure(EntityTypeBuilder<ProductImage> builder)
    {
        builder.ToTable("product_images", "public");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
            .HasColumnName("id");

        builder.Property(x => x.ProductId)
            .HasColumnName("product_id")
            .IsRequired();

        builder.Property(x => x.ImageUrl)
            .HasColumnName("image_url")
            .IsRequired();

        builder.Property(x => x.AltText)
            .HasColumnName("alt_text");

        builder.Property(x => x.DisplayOrder)
            .HasColumnName("display_order")
            .HasDefaultValue(0);

        builder.Property(x => x.IsMain)
            .HasColumnName("is_main")
            .HasDefaultValue(false);

        builder.Property(x => x.CreatedAt)
            .HasColumnName("created_at");

        builder.HasOne(x => x.Product)
            .WithMany(x => x.Images)
            .HasForeignKey(x => x.ProductId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(x => x.ProductId);
    }
}