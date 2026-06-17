using OrderTrack.Domain.Entities;

namespace OrderTrack.Application.Interfaces;

public interface IProductRepository
{
    Task<List<Product>> GetAllAsync(CancellationToken cancellationToken = default);

    Task<Product?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

    Task<Product> CreateAsync(Product product, CancellationToken cancellationToken = default);

    Task<Product?> UpdateAsync(
    Guid id,
    Product product,
    CancellationToken cancellationToken = default);

    Task<bool> DeactivateAsync(
    Guid id,
    CancellationToken cancellationToken = default);
}