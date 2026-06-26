namespace OrderTrack.Application.DTOs.Products;

public class ProductImageDto
{
    public Guid Id { get; set; }

    public string ImageUrl { get; set; } = string.Empty;

    public string? AltText { get; set; }

    public int DisplayOrder { get; set; }

    public bool IsMain { get; set; }
}