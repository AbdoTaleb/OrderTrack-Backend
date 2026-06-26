using Microsoft.EntityFrameworkCore;
using OrderTrack.Application.Interfaces;
using OrderTrack.Domain.Entities;
using OrderTrack.Infrastructure.Data;

namespace OrderTrack.Infrastructure.Repositories;

public class NonWorkingDayRepository : INonWorkingDayRepository
{
    private readonly OrderTrackDbContext _context;

    public NonWorkingDayRepository(OrderTrackDbContext context)
    {
        _context = context;
    }

    public async Task<List<NonWorkingDay>> GetBetweenDatesAsync(
        DateOnly startDate,
        DateOnly endDate,
        CancellationToken cancellationToken = default)
    {
        return await _context.NonWorkingDays
            .AsNoTracking()
            .Where(x => x.Date >= startDate && x.Date <= endDate)
            .ToListAsync(cancellationToken);
    }
}