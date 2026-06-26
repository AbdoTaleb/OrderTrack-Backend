using Microsoft.EntityFrameworkCore;
using OrderTrack.Domain.Entities;
using System.Collections.Generic;
using System.Reflection.Emit;

namespace OrderTrack.Infrastructure.Data;

public class OrderTrackDbContext : DbContext
{

    public OrderTrackDbContext(DbContextOptions<OrderTrackDbContext> options)
        : base(options)
    {
    }

    public DbSet<Order> Orders => Set<Order>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(OrderTrackDbContext).Assembly);

        base.OnModelCreating(modelBuilder);
    }

    public DbSet<Product> Products => Set<Product>();
    public DbSet<OrderItem> OrderItems => Set<OrderItem>();
    public DbSet<NonWorkingDay> NonWorkingDays => Set<NonWorkingDay>();

    public DbSet<ProductionAllocation> ProductionAllocations => Set<ProductionAllocation>();
    public DbSet<ProductImage> ProductImages => Set<ProductImage>();
}