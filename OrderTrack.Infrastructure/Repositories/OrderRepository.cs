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

    public async Task UpdateTotalProductionHoursAsync(
    Guid orderId,
    decimal hoursToAdd,
    CancellationToken cancellationToken = default)
    {
        const string sql = """
        update public.orders
        set
            total_production_hours = coalesce(total_production_hours, 0) + @hours_to_add,
            updated_at = now()
        where id = @order_id;
        """;

        await _context.Database.ExecuteSqlRawAsync(
            sql,
            new Npgsql.NpgsqlParameter("order_id", orderId),
            new Npgsql.NpgsqlParameter("hours_to_add", hoursToAdd)
        );
    }

    public async Task<Dictionary<DateOnly, decimal>> GetBookedProductionHoursByDateAsync(
    DateOnly startDate,
    DateOnly endDate,
    CancellationToken cancellationToken = default)
    {
        var result = await _context.ProductionAllocations
            .AsNoTracking()
            .Where(x =>
                x.Date >= startDate &&
                x.Date <= endDate)
            .GroupBy(x => x.Date)
            .Select(g => new
            {
                Date = g.Key,
                BookedHours = g.Sum(x => x.AllocatedHours)
            })
            .ToListAsync(cancellationToken);

        return result.ToDictionary(x => x.Date, x => x.BookedHours);
    }


    public async Task<Order> CreateWithItemsAsync(
    Order order,
    CancellationToken cancellationToken = default)
    {
        _context.Orders.Add(order);
        await _context.SaveChangesAsync(cancellationToken);

        return order;
    }
}