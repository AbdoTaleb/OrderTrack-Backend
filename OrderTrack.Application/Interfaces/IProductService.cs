using OrderTrack.Application.DTOs.Products;

namespace OrderTrack.Application.Interfaces;

public interface IProductService
{
    Task<List<ProductResponseDto>> GetAllAsync(CancellationToken cancellationToken = default);

    Task<ProductResponseDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

    Task<ProductResponseDto> CreateAsync(CreateProductDto dto, CancellationToken cancellationToken = default);

    Task<ProductResponseDto?> UpdateAsync(
    Guid id,
    UpdateProductDto dto,
    CancellationToken cancellationToken = default);

    Task<bool> DeactivateAsync(
    Guid id,
    CancellationToken cancellationToken = default);
}