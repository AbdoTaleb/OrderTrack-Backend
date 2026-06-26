using OrderTrack.Application.DTOs.Production;
using OrderTrack.Application.Interfaces;

namespace OrderTrack.Application.Services;

public class ProductionCalendarService : IProductionCalendarService
{
    private const decimal DailyCapacityHours = 8;

    private readonly INonWorkingDayRepository _nonWorkingDayRepository;
    private readonly IOrderRepository _orderRepository;

    public ProductionCalendarService(
        INonWorkingDayRepository nonWorkingDayRepository,
        IOrderRepository orderRepository)
    {
        _nonWorkingDayRepository = nonWorkingDayRepository;
        _orderRepository = orderRepository;
    }

    private static readonly HashSet<DayOfWeek> WorkingDays =
    [
        DayOfWeek.Monday,
        DayOfWeek.Tuesday,
        DayOfWeek.Wednesday,
        DayOfWeek.Thursday,
        DayOfWeek.Friday
    ];

    public async Task<List<ProductionDayDto>> GetCalendarAsync(
        DateOnly startDate,
        int days,
        CancellationToken cancellationToken = default)
    {
        var endDate = startDate.AddDays(days - 1);

        var bookedHoursByDate = await _orderRepository.GetBookedProductionHoursByDateAsync(
            startDate,
            endDate,
            cancellationToken);

        var nonWorkingDays = await _nonWorkingDayRepository.GetBetweenDatesAsync(
            startDate,
            endDate,
            cancellationToken);

        var nonWorkingDaysByDate = nonWorkingDays.ToDictionary(x => x.Date);

        var calendar = new List<ProductionDayDto>();

        for (var i = 0; i < days; i++)
        {
            var date = startDate.AddDays(i);

            var isWorkingDay = WorkingDays.Contains(date.DayOfWeek);

            var isCustomNonWorkingDay =
                nonWorkingDaysByDate.TryGetValue(date, out var nonWorkingDay);

            var isNonWorkingDay = !isWorkingDay || isCustomNonWorkingDay;

            var bookedHours = bookedHoursByDate.TryGetValue(date, out var hours)
                ? hours
                : 0;

            var capacityHours = isNonWorkingDay ? 0 : DailyCapacityHours;

            var remainingHours = Math.Max(0, capacityHours - bookedHours);

            calendar.Add(new ProductionDayDto
            {
                Date = date,
                CapacityHours = capacityHours,
                BookedHours = bookedHours,
                RemainingHours = remainingHours,
                IsNonWorkingDay = isNonWorkingDay,
                Reason = isCustomNonWorkingDay
                    ? nonWorkingDay?.Reason
                    : !isWorkingDay ? "Weekend" : null
            });
        }

        return calendar;
    }

    public async Task<SuggestedShippingDateDto> SuggestShippingDateAsync(
    SuggestShippingDateDto dto,
    CancellationToken cancellationToken = default)
    {
        if (dto.RequiredHours <= 0)
        {
            throw new InvalidOperationException("Required hours must be greater than zero.");
        }

        const int searchDays = 90;

        var calendar = await GetCalendarAsync(
            dto.StartDate,
            searchDays,
            cancellationToken);

        var remainingRequiredHours = dto.RequiredHours;
        var allocations = new List<ProductionAllocationDto>();

        foreach (var day in calendar)
        {
            if (day.IsNonWorkingDay || day.RemainingHours <= 0)
            {
                continue;
            }

            var allocatedHours = Math.Min(day.RemainingHours, remainingRequiredHours);

            allocations.Add(new ProductionAllocationDto
            {
                Date = day.Date,
                AllocatedHours = allocatedHours
            });

            remainingRequiredHours -= allocatedHours;

            if (remainingRequiredHours <= 0)
            {
                return new SuggestedShippingDateDto
                {
                    SuggestedShippingDate = day.Date,
                    RequiredHours = dto.RequiredHours,
                    Allocations = allocations
                };
            }
        }

        throw new InvalidOperationException("No available production date found in the next 90 days.");
    }
}