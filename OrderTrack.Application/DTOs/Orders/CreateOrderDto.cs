namespace OrderTrack.Application.DTOs.Orders;

public class CreateOrderDto
{
    public string CustomerName { get; set; } = string.Empty;

    public string? PhoneNumber { get; set; }

    public string? Address { get; set; }

    public string OrderDetails { get; set; } = string.Empty;

    public DateOnly? ReceivedDate { get; set; }

    public DateOnly? ShippingDate { get; set; }

    public string? PaymentMethod { get; set; }

    public decimal TotalAmount { get; set; }

    public string PaymentStatus { get; set; } = "cod";

    public decimal DepositAmount { get; set; }

    public string Status { get; set; } = "new";

    public Guid HandledBy { get; set; }
}