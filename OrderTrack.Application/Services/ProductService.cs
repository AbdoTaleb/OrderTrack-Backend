using OrderTrack.Application.DTOs.Products;
using OrderTrack.Application.Interfaces;
using OrderTrack.Domain.Entities;

namespace OrderTrack.Application.Services;

public class ProductService : IProductService
{
    private readonly IProductRepository _productRepository;

    public ProductService(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    public async Task<List<ProductResponseDto>> GetAllAsync(
        CancellationToken cancellationToken = default)
    {
        var products = await _productRepository.GetAllAsync(cancellationToken);

        return products
            .Select(MapToResponseDto)
            .ToList();
    }

    public async Task<ProductResponseDto?> GetByIdAsync(
        Guid id,
        CancellationToken cancellationToken = default)
    {
        var product = await _productRepository.GetByIdAsync(id, cancellationToken);

        return product is null ? null : MapToResponseDto(product);
    }

    public async Task<ProductResponseDto> CreateAsync(
        CreateProductDto dto,
        CancellationToken cancellationToken = default)
    {
        var product = new Product
        {
            Id = Guid.NewGuid(),
            Name = dto.Name.Trim(),
            ProductionHours = dto.ProductionHours,
            IsAvailable = dto.IsAvailable,
            StockQuantity = dto.StockQuantity,
            IsActive = true,
            IsVariableQuantity = dto.IsVariableQuantity,
            MinimumQuantity = dto.IsVariableQuantity ? dto.MinimumQuantity : null,
            QuantityStep = dto.IsVariableQuantity ? dto.QuantityStep : null,
            HoursPerStep = dto.IsVariableQuantity ? dto.HoursPerStep : null,
            CreatedAt = DateTimeOffset.UtcNow
        };

        var created = await _productRepository.CreateAsync(product, cancellationToken);

        return MapToResponseDto(created);
    }

    public async Task<ProductResponseDto?> UpdateAsync(
    Guid id,
    UpdateProductDto dto,
    CancellationToken cancellationToken = default)
    {
        var existingProduct = await _productRepository.GetByIdAsync(id, cancellationToken);

        if (existingProduct is null)
        {
            return null;
        }

        var productToUpdate = new Product
        {
            Id = id,
            Name = dto.Name.Trim(),
            ProductionHours = dto.ProductionHours,
            IsAvailable = dto.IsAvailable,
            StockQuantity = dto.StockQuantity,
            IsActive = true,
            IsVariableQuantity = dto.IsVariableQuantity,
            MinimumQuantity = dto.IsVariableQuantity ? dto.MinimumQuantity : null,
            QuantityStep = dto.IsVariableQuantity ? dto.QuantityStep : null,
            HoursPerStep = dto.IsVariableQuantity ? dto.HoursPerStep : null,
            CreatedAt = existingProduct.CreatedAt,
            UpdatedAt = DateTimeOffset.UtcNow
        };

        var updatedProduct = await _productRepository.UpdateAsync(
            id,
            productToUpdate,
            cancellationToken);

        return updatedProduct is null ? null : MapToResponseDto(updatedProduct);
    }
    public async Task<bool> DeactivateAsync(
    Guid id,
    CancellationToken cancellationToken = default)
    {
        return await _productRepository.DeactivateAsync(id, cancellationToken);
    }

    private static ProductResponseDto MapToResponseDto(Product product)
    {
        return new ProductResponseDto
        {
            Id = product.Id,
            Name = product.Name,
            ProductionHours = product.ProductionHours,
            IsAvailable = product.IsAvailable,
            StockQuantity = product.StockQuantity,
            IsActive = product.IsActive,
            IsVariableQuantity = product.IsVariableQuantity,
            MinimumQuantity = product.MinimumQuantity,
            QuantityStep = product.QuantityStep,
            HoursPerStep = product.HoursPerStep,
            CreatedAt = product.CreatedAt,
            UpdatedAt = product.UpdatedAt
        };
    }
}