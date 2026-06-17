using OrderTrack.Domain.Enums;

namespace OrderTrack.Domain.Entities;

public class Order
{
    public Guid Id { get; set; }
    public ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();

    public long OrderNumber { get; set; }

    public string CustomerName { get; set; } = string.Empty;

    public string? PhoneNumber { get; set; }

    public string? Address { get; set; }

    public string OrderDetails { get; set; } = string.Empty;

    public DateOnly? ReceivedDate { get; set; }

    public DateOnly? ShippingDate { get; set; }

    public string? PaymentMethod { get; set; }

    public decimal TotalAmount { get; set; }

    public PaymentStatus PaymentStatus { get; set; }

    public decimal DepositAmount { get; set; }

    public OrderStatus Status { get; set; }

    public Guid HandledBy { get; set; }

    public DateTimeOffset? CreatedAt { get; set; }

    public DateTimeOffset? UpdatedAt { get; set; }
}