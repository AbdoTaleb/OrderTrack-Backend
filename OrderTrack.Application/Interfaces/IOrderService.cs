using OrderTrack.Application.DTOs.Orders;
using OrderTrack.Domain.Enums;

namespace OrderTrack.Application.Interfaces;

public interface IOrderService
{
    Task<List<OrderResponseDto>> GetAllAsync(OrderStatus? status = null, CancellationToken cancellationToken = default);

    Task<OrderResponseDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

    Task<OrderResponseDto> CreateAsync(CreateOrderDto dto, CancellationToken cancellationToken = default);
}