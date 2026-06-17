using OrderTrack.Domain.Entities;
using OrderTrack.Domain.Enums;

namespace OrderTrack.Application.Interfaces;

public interface IOrderRepository
{
    Task<List<Order>> GetAllAsync(OrderStatus? status = null, CancellationToken cancellationToken = default);

    Task<Order?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<Order> CreateAsync(Order order, CancellationToken cancellationToken = default);
}