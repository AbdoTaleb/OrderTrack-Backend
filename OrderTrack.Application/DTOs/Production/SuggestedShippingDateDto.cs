namespace OrderTrack.Application.DTOs.Production;

public class SuggestedShippingDateDto
{
    public DateOnly SuggestedShippingDate { get; set; }

    public decimal RequiredHours { get; set; }

    public List<ProductionAllocationDto> Allocations { get; set; } = new();
}