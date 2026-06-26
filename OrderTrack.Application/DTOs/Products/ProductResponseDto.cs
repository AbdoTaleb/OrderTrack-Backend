namespace OrderTrack.Application.DTOs.Products;

public class ProductResponseDto
{
    public Guid Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public decimal ProductionHours { get; set; }

    public bool IsAvailable { get; set; }

    public int? StockQuantity { get; set; }

    public bool IsActive { get; set; }

    public bool IsVariableQuantity { get; set; }

    public int? MinimumQuantity { get; set; }

    public int? QuantityStep { get; set; }

    public decimal? HoursPerStep { get; set; }

    public DateTimeOffset CreatedAt { get; set; }

    public DateTimeOffset? UpdatedAt { get; set; }
    public List<ProductImageDto> Images { get; set; } = new();
}