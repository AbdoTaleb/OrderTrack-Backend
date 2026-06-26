namespace OrderTrack.Application.DTOs.Production;

public class ProductionDayDto
{
    public DateOnly Date { get; set; }

    public decimal CapacityHours { get; set; }

    public decimal BookedHours { get; set; }

    public decimal RemainingHours { get; set; }

    public bool IsNonWorkingDay { get; set; }

    public string? Reason { get; set; }
}