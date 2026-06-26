namespace OrderTrack.Application.DTOs.Production;

public class SuggestShippingDateDto
{
    public DateOnly StartDate { get; set; }

    public decimal RequiredHours { get; set; }
}