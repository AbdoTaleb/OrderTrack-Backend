using Microsoft.EntityFrameworkCore;
using OrderTrack.Application.Interfaces;
using OrderTrack.Domain.Entities;
using OrderTrack.Domain.Enums;
using OrderTrack.Infrastructure.Data;

namespace OrderTrack.Infrastructure.Repositories;

public class OrderRepository : IOrderRepository
{
    private readonly OrderTrackDbContext _context;

    public OrderRepository(OrderTrackDbContext context)
    {
        _context = context;
    }

    public async Task<List<Order>> GetAllAsync(
        OrderStatus? status = null,
        CancellationToken cancellationToken = default)
    {
        var query = _context.Orders
            .AsNoTracking()
            .AsQueryable();

        if (status is not null)
        {
            query = query.Where(o => o.Status == status);
        }
        else
        {
            query = query.Where(o => o.Status != OrderStatus.Canceled);
        }

        return await query
            .OrderByDescending(o => o.CreatedAt)
            .ToListAsync(cancellationToken);
    }

    public async Task<Order?> GetByIdAsync(
        Guid id,
        CancellationToken cancellationToken = default)
    {
        return await _context.Orders
            .AsNoTracking()
            .FirstOrDefaultAsync(o => o.Id == id, cancellationToken);
    }

    public async Task<Order> CreateAsync(
    Order order,
    CancellationToken cancellationToken = default)
    {
        _context.Orders.Add(order);
        await _context.SaveChangesAsync(cancellationToken);

        return order;
    }
}