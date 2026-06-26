namespace OrderTrack.Application.DTOs.Orders;

public class CreateOrderItemInputDto
{
    public Guid ProductId { get; set; }

    public int Quantity { get; set; }
}