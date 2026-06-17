using OrderTrack.Application.DTOs.Orders;
using OrderTrack.Application.Interfaces;
using OrderTrack.Domain.Entities;
using OrderTrack.Domain.Enums;

namespace OrderTrack.Application.Services;

public class OrderService : IOrderService
{
    private readonly IOrderRepository _orderRepository;

    public OrderService(IOrderRepository orderRepository)
    {
        _orderRepository = orderRepository;
    }

    public async Task<List<OrderResponseDto>> GetAllAsync(
        OrderStatus? status = null,
        CancellationToken cancellationToken = default)
    {
        var orders = await _orderRepository.GetAllAsync(status, cancellationToken);

        return orders
            .Select(MapToResponseDto)
            .ToList();
    }

    public async Task<OrderResponseDto?> GetByIdAsync(
        Guid id,
        CancellationToken cancellationToken = default)
    {
        var order = await _orderRepository.GetByIdAsync(id, cancellationToken);

        return order is null ? null : MapToResponseDto(order);
    }

    private static OrderResponseDto MapToResponseDto(Order order)
    {
        return new OrderResponseDto
        {
            Id = order.Id,
            OrderNumber = order.OrderNumber,
            CustomerName = order.CustomerName,
            PhoneNumber = order.PhoneNumber,
            Address = order.Address,
            OrderDetails = order.OrderDetails,
            ReceivedDate = order.ReceivedDate,
            ShippingDate = order.ShippingDate,
            PaymentMethod = order.PaymentMethod,
            TotalAmount = order.TotalAmount,
            PaymentStatus = order.PaymentStatus switch
            {
                Domain.Enums.PaymentStatus.PaidFull => "paid_full",
                Domain.Enums.PaymentStatus.Deposit => "deposit",
                Domain.Enums.PaymentStatus.Cod => "cod",
                _ => "cod"
            },
            DepositAmount = order.DepositAmount,
            Status = order.Status switch
            {
                Domain.Enums.OrderStatus.New => "new",
                Domain.Enums.OrderStatus.InProgress => "in_progress",
                Domain.Enums.OrderStatus.Shipped => "shipped",
                Domain.Enums.OrderStatus.Delivered => "delivered",
                Domain.Enums.OrderStatus.Canceled => "canceled",
                _ => "new"
            },
            HandledBy = order.HandledBy,
            CreatedAt = order.CreatedAt,
            UpdatedAt = order.UpdatedAt
        };
    }

    public async Task<OrderResponseDto> CreateAsync(
    CreateOrderDto dto,
    CancellationToken cancellationToken = default)
    {
        var order = new Order
        {
            Id = Guid.NewGuid(),
            CustomerName = dto.CustomerName.Trim(),
            PhoneNumber = string.IsNullOrWhiteSpace(dto.PhoneNumber) ? null : dto.PhoneNumber.Trim(),
            Address = string.IsNullOrWhiteSpace(dto.Address) ? null : dto.Address.Trim(),
            OrderDetails = dto.OrderDetails.Trim(),
            ReceivedDate = dto.ReceivedDate ?? DateOnly.FromDateTime(DateTime.UtcNow),
            ShippingDate = dto.ShippingDate,
            PaymentMethod = string.IsNullOrWhiteSpace(dto.PaymentMethod) ? null : dto.PaymentMethod.Trim(),
            TotalAmount = dto.TotalAmount,
            PaymentStatus = ParsePaymentStatus(dto.PaymentStatus),
            DepositAmount = dto.PaymentStatus == "deposit" ? dto.DepositAmount : 0,
            Status = ParseOrderStatus(dto.Status),
            HandledBy = dto.HandledBy,
            CreatedAt = DateTimeOffset.UtcNow,
            UpdatedAt = DateTimeOffset.UtcNow
        };

        var created = await _orderRepository.CreateAsync(order, cancellationToken);

        return MapToResponseDto(created);
    }


    private static OrderTrack.Domain.Enums.PaymentStatus ParsePaymentStatus(string value)
    {
        return value switch
        {
            "paid_full" => OrderTrack.Domain.Enums.PaymentStatus.PaidFull,
            "deposit" => OrderTrack.Domain.Enums.PaymentStatus.Deposit,
            "cod" => OrderTrack.Domain.Enums.PaymentStatus.Cod,
            _ => OrderTrack.Domain.Enums.PaymentStatus.Cod
        };
    }

    private static OrderTrack.Domain.Enums.OrderStatus ParseOrderStatus(string value)
    {
        return value switch
        {
            "new" => OrderTrack.Domain.Enums.OrderStatus.New,
            "in_progress" => OrderTrack.Domain.Enums.OrderStatus.InProgress,
            "shipped" => OrderTrack.Domain.Enums.OrderStatus.Shipped,
            "delivered" => OrderTrack.Domain.Enums.OrderStatus.Delivered,
            "canceled" => OrderTrack.Domain.Enums.OrderStatus.Canceled,
            _ => OrderTrack.Domain.Enums.OrderStatus.New
        };
    }
}