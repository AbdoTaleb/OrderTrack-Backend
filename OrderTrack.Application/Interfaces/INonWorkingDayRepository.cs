using OrderTrack.Domain.Entities;

namespace OrderTrack.Application.Interfaces;

public interface INonWorkingDayRepository
{
    Task<List<NonWorkingDay>> GetBetweenDatesAsync(
        DateOnly startDate,
        DateOnly endDate,
        CancellationToken cancellationToken = default);
}