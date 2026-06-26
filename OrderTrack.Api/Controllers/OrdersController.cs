using Microsoft.AspNetCore.Mvc;
using OrderTrack.Application.DTOs.Orders;
using OrderTrack.Application.Interfaces;
using OrderTrack.Domain.Enums;

namespace OrderTrack.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class OrdersController : ControllerBase
{
    private readonly IOrderService _orderService;

    public OrdersController(IOrderService orderService)
    {
        _orderService = orderService;
    }

    [HttpGet]
    public async Task<ActionResult<List<OrderResponseDto>>> GetAll(
        [FromQuery] OrderStatus? status,
        CancellationToken cancellationToken)
    {
        var orders = await _orderService.GetAllAsync(status, cancellationToken);
        return Ok(orders);
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<OrderResponseDto>> GetById(
        Guid id,
        CancellationToken cancellationToken)
    {
        var order = await _orderService.GetByIdAsync(id, cancellationToken);

        if (order is null)
        {
            return NotFound();
        }

        return Ok(order);
    }

    [HttpPost]
    public async Task<ActionResult<OrderResponseDto>> Create(
    CreateOrderDto dto,
    CancellationToken cancellationToken)
    {
        var createdOrder = await _orderService.CreateAsync(dto, cancellationToken);

        return CreatedAtAction(
            nameof(GetById),
            new { id = createdOrder.Id },
            createdOrder);
    }

    [HttpPost("with-items")]
    public async Task<ActionResult<OrderResponseDto>> CreateWithItems(
    [FromBody] CreateOrderWithItemsDto dto,
    CancellationToken cancellationToken)
    {
        try
        {
            var createdOrder = await _orderService.CreateWithItemsAsync(
                dto,
                cancellationToken);

            return CreatedAtAction(
                nameof(GetById),
                new { id = createdOrder.Id },
                createdOrder);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ex.Message);
        }
    }
}