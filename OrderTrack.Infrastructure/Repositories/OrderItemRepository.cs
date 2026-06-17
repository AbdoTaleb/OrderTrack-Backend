using Microsoft.EntityFrameworkCore;
using OrderTrack.Application.Interfaces;
using OrderTrack.Domain.Entities;
using OrderTrack.Infrastructure.Data;

namespace OrderTrack.Infrastructure.Repositories;

public class OrderItemRepository : IOrderItemRepository
{
    private readonly OrderTrackDbContext _context;

    public OrderItemRepository(OrderTrackDbContext context)
    {
        _context = context;
    }

    public async Task<List<OrderItem>> GetByOrderIdAsync(
        Guid orderId,
        CancellationToken cancellationToken = default)
    {
        return await _context.OrderItems
            .AsNoTracking()
            .Include(x => x.Product)
            .Where(x => x.OrderId == orderId)
            .OrderBy(x => x.CreatedAt)
            .ToListAsync(cancellationToken);
    }

    public async Task<OrderItem> CreateAsync(
        OrderItem orderItem,
        CancellationToken cancellationToken = default)
    {
        _context.OrderItems.Add(orderItem);
        await _context.SaveChangesAsync(cancellationToken);

        return orderItem;
    }
}
