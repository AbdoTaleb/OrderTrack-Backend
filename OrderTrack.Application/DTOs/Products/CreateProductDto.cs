namespace OrderTrack.Application.DTOs.Products;

public class CreateProductDto
{
    public string Name { get; set; } = string.Empty;

    public decimal ProductionHours { get; set; }

    public bool IsAvailable { get; set; } = true;

    public int? StockQuantity { get; set; }

    public bool IsVariableQuantity { get; set; } = false;

    public int? MinimumQuantity { get; set; }

    public int? QuantityStep { get; set; }

    public decimal? HoursPerStep { get; set; }
}