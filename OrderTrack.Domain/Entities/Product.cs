namespace OrderTrack.Domain.Entities;

public class Product
{
    public Guid Id { get; set; }
    public ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();

    public string Name { get; set; } = string.Empty;

    public decimal ProductionHours { get; set; }

    public bool IsAvailable { get; set; } = true;

    public int? StockQuantity { get; set; }

    public bool IsActive { get; set; } = true;

    public bool IsVariableQuantity { get; set; } = false;

    public int? MinimumQuantity { get; set; }

    public int? QuantityStep { get; set; }

    public decimal? HoursPerStep { get; set; }

    public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;

    public DateTimeOffset? UpdatedAt { get; set; }

    public ICollection<ProductImage> Images { get; set; } = new List<ProductImage>();
}