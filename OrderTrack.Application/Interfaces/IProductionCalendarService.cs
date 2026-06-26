using OrderTrack.Application.DTOs.Production;

namespace OrderTrack.Application.Interfaces;

public interface IProductionCalendarService
{
    Task<List<ProductionDayDto>> GetCalendarAsync(
        DateOnly startDate,
        int days,
        CancellationToken cancellationToken = default);

    Task<SuggestedShippingDateDto> SuggestShippingDateAsync(
    SuggestShippingDateDto dto,
    CancellationToken cancellationToken = default);
}