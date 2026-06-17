using Microsoft.AspNetCore.Mvc;
using OrderTrack.Application.DTOs.OrderItems;
using OrderTrack.Application.Interfaces;

namespace OrderTrack.Api.Controllers;

[ApiController]
[Route("api/orders/{orderId:guid}/items")]
public class OrderItemsController : ControllerBase
{
    private readonly IOrderItemService _orderItemService;

    public OrderItemsController(IOrderItemService orderItemService)
    {
        _orderItemService = orderItemService;
    }

    [HttpGet]
    public async Task<ActionResult<List<OrderItemResponseDto>>> GetByOrderId(
        Guid orderId,
        CancellationToken cancellationToken)
    {
        var items = await _orderItemService.GetByOrderIdAsync(orderId, cancellationToken);
        return Ok(items);
    }

    [HttpPost]
    public async Task<ActionResult<OrderItemResponseDto>> Create(
        Guid orderId,
        CreateOrderItemDto dto,
        CancellationToken cancellationToken)
    {
        try
        {
            var createdItem = await _orderItemService.CreateAsync(
                orderId,
                dto,
                cancellationToken);

            return CreatedAtAction(
                nameof(GetByOrderId),
                new { orderId },
                createdItem);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ex.Message);
        }
    }
}