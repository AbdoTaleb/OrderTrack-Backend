using Microsoft.AspNetCore.Mvc;
using OrderTrack.Application.DTOs.Production;
using OrderTrack.Application.Interfaces;

namespace OrderTrack.Api.Controllers;

[ApiController]
[Route("api/production-calendar")]
public class ProductionCalendarController : ControllerBase
{
    private readonly IProductionCalendarService _productionCalendarService;

    public ProductionCalendarController(IProductionCalendarService productionCalendarService)
    {
        _productionCalendarService = productionCalendarService;
    }

    [HttpGet]
    public async Task<ActionResult<List<ProductionDayDto>>> GetCalendar(
    [FromQuery] DateOnly? startDate,
    [FromQuery] int days = 14,
    CancellationToken cancellationToken = default)
    {
        var actualStartDate = startDate ?? DateOnly.FromDateTime(DateTime.Today);

        var calendar = await _productionCalendarService.GetCalendarAsync(
            actualStartDate,
            days,
            cancellationToken);

        return Ok(calendar);
    }

    [HttpPost("suggest-shipping-date")]
    public async Task<ActionResult<SuggestedShippingDateDto>> SuggestShippingDate(
    SuggestShippingDateDto dto,
    CancellationToken cancellationToken)
    {
        try
        {
            var result = await _productionCalendarService.SuggestShippingDateAsync(
                dto,
                cancellationToken);

            return Ok(result);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ex.Message);
        }
    }
}