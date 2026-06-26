using Microsoft.EntityFrameworkCore;
using Npgsql;
using OrderTrack.Application.Interfaces;
using OrderTrack.Domain.Entities;
using OrderTrack.Infrastructure.Data;

namespace OrderTrack.Infrastructure.Repositories;

public class ProductRepository : IProductRepository
{
    private readonly OrderTrackDbContext _context;

    public ProductRepository(OrderTrackDbContext context)
    {
        _context = context;
    }

    public async Task<List<Product>> GetAllAsync(
        CancellationToken cancellationToken = default)
    {
        return await _context.Products
            .AsNoTracking()
            .Include(p => p.Images)
            .Where(p => p.IsActive)
            .OrderBy(p => p.Name)
            .ToListAsync(cancellationToken);
    }

    public async Task<Product?> GetByIdAsync(
        Guid id,
        CancellationToken cancellationToken = default)
    {
        return await _context.Products
            .AsNoTracking()
            .Include(p => p.Images)
            .FirstOrDefaultAsync(p => p.Id == id && p.IsActive, cancellationToken);
    }

    public async Task<Product> CreateAsync(
    Product product,
    CancellationToken cancellationToken = default)
    {
        const string sql = """
        insert into public.products
        (
            id,
            name,
            production_hours,
            is_available,
            stock_quantity,
            is_active,
            is_variable_quantity,
            minimum_quantity,
            quantity_step,
            hours_per_step,
            created_at,
            updated_at
        )
        values
        (
            @id,
            @name,
            @production_hours,
            @is_available,
            @stock_quantity,
            @is_active,
            @is_variable_quantity,
            @minimum_quantity,
            @quantity_step,
            @hours_per_step,
            @created_at,
            @updated_at
        );
        """;

        await _context.Database.ExecuteSqlRawAsync(
            sql,
            new NpgsqlParameter("id", product.Id),
            new NpgsqlParameter("name", product.Name),
            new NpgsqlParameter("production_hours", product.ProductionHours),
            new NpgsqlParameter("is_available", product.IsAvailable),
            new NpgsqlParameter("stock_quantity", (object?)product.StockQuantity ?? DBNull.Value),
            new NpgsqlParameter("is_active", product.IsActive),
            new NpgsqlParameter("is_variable_quantity", product.IsVariableQuantity),
            new NpgsqlParameter("minimum_quantity", (object?)product.MinimumQuantity ?? DBNull.Value),
            new NpgsqlParameter("quantity_step", (object?)product.QuantityStep ?? DBNull.Value),
            new NpgsqlParameter("hours_per_step", (object?)product.HoursPerStep ?? DBNull.Value),
            new NpgsqlParameter("created_at", product.CreatedAt),
            new NpgsqlParameter("updated_at", (object?)product.UpdatedAt ?? DBNull.Value)
        );

        return product;
    }

    public async Task<Product?> UpdateAsync(
    Guid id,
    Product product,
    CancellationToken cancellationToken = default)
    {
        const string sql = """
        update public.products
        set
            name = @name,
            production_hours = @production_hours,
            is_available = @is_available,
            stock_quantity = @stock_quantity,
            is_variable_quantity = @is_variable_quantity,
            minimum_quantity = @minimum_quantity,
            quantity_step = @quantity_step,
            hours_per_step = @hours_per_step,
            updated_at = @updated_at
        where id = @id;
        """;

        var rowsAffected = await _context.Database.ExecuteSqlRawAsync(
            sql,
            new Npgsql.NpgsqlParameter("id", id),
            new Npgsql.NpgsqlParameter("name", product.Name),
            new Npgsql.NpgsqlParameter("production_hours", product.ProductionHours),
            new Npgsql.NpgsqlParameter("is_available", product.IsAvailable),
            new Npgsql.NpgsqlParameter("stock_quantity", (object?)product.StockQuantity ?? DBNull.Value),
            new Npgsql.NpgsqlParameter("is_variable_quantity", product.IsVariableQuantity),
            new Npgsql.NpgsqlParameter("minimum_quantity", (object?)product.MinimumQuantity ?? DBNull.Value),
            new Npgsql.NpgsqlParameter("quantity_step", (object?)product.QuantityStep ?? DBNull.Value),
            new Npgsql.NpgsqlParameter("hours_per_step", (object?)product.HoursPerStep ?? DBNull.Value),
            new Npgsql.NpgsqlParameter("updated_at", DateTimeOffset.UtcNow)
        );

        if (rowsAffected == 0)
        {
            return null;
        }

        return await GetByIdAsync(id, cancellationToken);
    }

    public async Task<bool> DeactivateAsync(
    Guid id,
    CancellationToken cancellationToken = default)
    {
        const string sql = """
        update public.products
        set
            is_active = false,
            is_available = false,
            updated_at = now()
        where id = @id;
        """;

        var rowsAffected = await _context.Database.ExecuteSqlRawAsync(
            sql,
            new Npgsql.NpgsqlParameter("id", id)
        );

        return rowsAffected > 0;
    }
}