namespace OrderTrack.Domain.Entities;

public class NonWorkingDay
{
    public Guid Id { get; set; }

    public DateOnly Date { get; set; }

    public string? Reason { get; set; }

    public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;
}