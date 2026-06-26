namespace OrderTrack.Application.DTOs.Orders;

public class CreateOrderWithItemsDto
{
    public string CustomerName { get; set; } = string.Empty;

    public string? PhoneNumber { get; set; }

    public string? Address { get; set; }

    public string OrderDetails { get; set; } = string.Empty;

    public DateOnly? ReceivedDate { get; set; }

    public string? PaymentMethod { get; set; }

    public decimal TotalAmount { get; set; }

    public string PaymentStatus { get; set; } = "cod";

    public decimal DepositAmount { get; set; }

    public string Status { get; set; } = "new";

    public Guid HandledBy { get; set; }

    public List<CreateOrderItemInputDto> Items { get; set; } = new();
}