namespace OrderTrack.Domain.Entities;

public class ProductionAllocation
{
    public Guid Id { get; set; }

    public Guid OrderId { get; set; }

    public DateOnly Date { get; set; }

    public decimal AllocatedHours { get; set; }

    public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;

    public Order? Order { get; set; }
}