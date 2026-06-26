namespace OrderTrack.Application.DTOs.Orders;

public class OrderResponseDto
{
    public Guid Id { get; set; }

    public long OrderNumber { get; set; }

    public string CustomerName { get; set; } = string.Empty;

    public string? PhoneNumber { get; set; }

    public string? Address { get; set; }

    public string OrderDetails { get; set; } = string.Empty;

    public DateOnly? ReceivedDate { get; set; }

    public DateOnly? ShippingDate { get; set; }

    public string? PaymentMethod { get; set; }

    public decimal TotalAmount { get; set; }
    public decimal TotalProductionHours { get; set; }

    public string PaymentStatus { get; set; } = string.Empty;

    public decimal DepositAmount { get; set; }

    public string Status { get; set; } = string.Empty;

    public Guid HandledBy { get; set; }

    public DateTimeOffset? CreatedAt { get; set; }

    public DateTimeOffset? UpdatedAt { get; set; }
}