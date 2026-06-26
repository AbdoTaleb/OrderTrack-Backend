using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OrderTrack.Domain.Entities;

namespace OrderTrack.Infrastructure.Configurations;

public class NonWorkingDayConfiguration : IEntityTypeConfiguration<NonWorkingDay>
{
    public void Configure(EntityTypeBuilder<NonWorkingDay> builder)
    {
        builder.ToTable("non_working_days", "public");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
            .HasColumnName("id");

        builder.Property(x => x.Date)
            .HasColumnName("date")
            .IsRequired();

        builder.Property(x => x.Reason)
            .HasColumnName("reason");

        builder.Property(x => x.CreatedAt)
            .HasColumnName("created_at");

        builder.HasIndex(x => x.Date)
            .IsUnique();
    }
}