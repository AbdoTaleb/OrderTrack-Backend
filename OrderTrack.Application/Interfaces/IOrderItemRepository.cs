using OrderTrack.Domain.Entities;

namespace OrderTrack.Application.Interfaces;

public interface IOrderItemRepository
{
    Task<List<OrderItem>> GetByOrderIdAsync(
        Guid orderId,
        CancellationToken cancellationToken = default);

    Task<OrderItem> CreateAsync(
        OrderItem orderItem,
        CancellationToken cancellationToken = default);
}