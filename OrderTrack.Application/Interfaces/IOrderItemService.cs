using OrderTrack.Application.DTOs.OrderItems;

namespace OrderTrack.Application.Interfaces;

public interface IOrderItemService
{
    Task<List<OrderItemResponseDto>> GetByOrderIdAsync(
        Guid orderId,
        CancellationToken cancellationToken = default);

    Task<OrderItemResponseDto> CreateAsync(
        Guid orderId,
        CreateOrderItemDto dto,
        CancellationToken cancellationToken = default);
}