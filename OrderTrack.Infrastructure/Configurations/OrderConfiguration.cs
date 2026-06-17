using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OrderTrack.Domain.Entities;
using OrderTrack.Domain.Enums;

namespace OrderTrack.Infrastructure.Configurations;

public class OrderConfiguration : IEntityTypeConfiguration<Order>
{
    public void Configure(EntityTypeBuilder<Order> builder)
    {
        builder.ToTable("orders", "public");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
            .HasColumnName("id");

        builder.Property(x => x.OrderNumber)
            .HasColumnName("order_number")
            .ValueGeneratedOnAdd();

        builder.Property(x => x.CustomerName)
            .HasColumnName("customer_name")
            .IsRequired();

        builder.Property(x => x.PhoneNumber)
            .HasColumnName("phone_number");

        builder.Property(x => x.Address)
            .HasColumnName("address");

        builder.Property(x => x.OrderDetails)
            .HasColumnName("order_details")
            .IsRequired();

        builder.Property(x => x.ReceivedDate)
            .HasColumnName("received_date");

        builder.Property(x => x.ShippingDate)
            .HasColumnName("shipping_date");

        builder.Property(x => x.PaymentMethod)
            .HasColumnName("payment_method");

        builder.Property(x => x.TotalAmount)
            .HasColumnName("total_amount")
            .HasColumnType("numeric(12,2)");

        builder.Property(x => x.PaymentStatus)
            .HasColumnName("payment_status")
            .HasConversion(
                v => ToDatabasePaymentStatus(v),
                v => FromDatabasePaymentStatus(v)
            );

        builder.Property(x => x.DepositAmount)
            .HasColumnName("deposit_amount")
            .HasColumnType("numeric(12,2)");

        builder.Property(x => x.Status)
            .HasColumnName("status")
            .HasConversion(
                v => ToDatabaseOrderStatus(v),
                v => FromDatabaseOrderStatus(v)
            );

        builder.Property(x => x.HandledBy)
            .HasColumnName("handled_by");

        builder.Property(x => x.CreatedAt)
            .HasColumnName("created_at");

        builder.Property(x => x.UpdatedAt)
            .HasColumnName("updated_at");
    }

    private static string ToDatabasePaymentStatus(PaymentStatus status)
    {
        return status switch
        {
            PaymentStatus.PaidFull => "paid_full",
            PaymentStatus.Deposit => "deposit",
            PaymentStatus.Cod => "cod",
            _ => "cod"
        };
    }

    private static PaymentStatus FromDatabasePaymentStatus(string status)
    {
        return status switch
        {
            "paid_full" => PaymentStatus.PaidFull,
            "deposit" => PaymentStatus.Deposit,
            "cod" => PaymentStatus.Cod,
            _ => PaymentStatus.Cod
        };
    }

    private static string ToDatabaseOrderStatus(OrderStatus status)
    {
        return status switch
        {
            OrderStatus.New => "new",
            OrderStatus.InProgress => "in_progress",
            OrderStatus.Shipped => "shipped",
            OrderStatus.Delivered => "delivered",
            OrderStatus.Canceled => "canceled",
            _ => "new"
        };
    }

    private static OrderStatus FromDatabaseOrderStatus(string status)
    {
        return status switch
        {
            "new" => OrderStatus.New,
            "in_progress" => OrderStatus.InProgress,
            "shipped" => OrderStatus.Shipped,
            "delivered" => OrderStatus.Delivered,
            "canceled" => OrderStatus.Canceled,
            _ => OrderStatus.New
        };
    }
}