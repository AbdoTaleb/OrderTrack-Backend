using OrderTrack.Application.DTOs.OrderItems;
using OrderTrack.Application.Interfaces;
using OrderTrack.Domain.Entities;

namespace OrderTrack.Application.Services;

public class OrderItemService : IOrderItemService
{
    private readonly IOrderItemRepository _orderItemRepository;
    private readonly IProductRepository _productRepository;
    private readonly IOrderRepository _orderRepository;

    public OrderItemService(
    IOrderItemRepository orderItemRepository,
    IProductRepository productRepository,
    IOrderRepository orderRepository)
    {
        _orderItemRepository = orderItemRepository;
        _productRepository = productRepository;
        _orderRepository = orderRepository;
    }

    public async Task<List<OrderItemResponseDto>> GetByOrderIdAsync(
        Guid orderId,
        CancellationToken cancellationToken = default)
    {
        var items = await _orderItemRepository.GetByOrderIdAsync(orderId, cancellationToken);

        return items.Select(MapToResponseDto).ToList();
    }

    public async Task<OrderItemResponseDto> CreateAsync(
        Guid orderId,
        CreateOrderItemDto dto,
        CancellationToken cancellationToken = default)
    {
        var product = await _productRepository.GetByIdAsync(dto.ProductId, cancellationToken);

        if (product is null)
        {
            throw new InvalidOperationException("Product not found.");
        }

        var totalHours = CalculateProductionHours(
            dto.Quantity,
            product.ProductionHours,
            product.IsVariableQuantity,
            product.MinimumQuantity,
            product.QuantityStep,
            product.HoursPerStep);

        var orderItem = new OrderItem
        {
            Id = Guid.NewGuid(),
            OrderId = orderId,
            ProductId = product.Id,
            Quantity = dto.Quantity,
            UnitProductionHours = product.ProductionHours,
            TotalProductionHours = totalHours,
            CreatedAt = DateTimeOffset.UtcNow
        };

        var created = await _orderItemRepository.CreateAsync(orderItem, cancellationToken);

        await _orderRepository.UpdateTotalProductionHoursAsync(
            orderId,
            created.TotalProductionHours,
            cancellationToken);

        created.Product = product;

        return MapToResponseDto(created);
    }

    private static decimal CalculateProductionHours(
        int quantity,
        decimal productionHours,
        bool isVariableQuantity,
        int? minimumQuantity,
        int? quantityStep,
        decimal? hoursPerStep)
    {
        if (quantity <= 0)
        {
            throw new InvalidOperationException("Quantity must be greater than zero.");
        }

        if (!isVariableQuantity)
        {
            return productionHours * quantity;
        }

        var min = minimumQuantity ?? 1;
        var step = quantityStep ?? 1;
        var stepHours = hoursPerStep ?? productionHours;

        if (quantity < min)
        {
            throw new InvalidOperationException($"Minimum quantity is {min}.");
        }

        var steps = (int)Math.Ceiling((decimal)quantity / step);

        return steps * stepHours;
    }

    private static OrderItemResponseDto MapToResponseDto(OrderItem item)
    {
        return new OrderItemResponseDto
        {
            Id = item.Id,
            OrderId = item.OrderId,
            ProductId = item.ProductId,
            ProductName = item.Product?.Name ?? string.Empty,
            Quantity = item.Quantity,
            UnitProductionHours = item.UnitProductionHours,
            TotalProductionHours = item.TotalProductionHours,
            CreatedAt = item.CreatedAt,
            UpdatedAt = item.UpdatedAt
        };
    }
}