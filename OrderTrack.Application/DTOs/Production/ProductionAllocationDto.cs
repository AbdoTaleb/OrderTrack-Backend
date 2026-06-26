namespace OrderTrack.Application.DTOs.Production;

public class ProductionAllocationDto
{
    public DateOnly Date { get; set; }

    public decimal AllocatedHours { get; set; }
}