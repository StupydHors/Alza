using Domain.Entities;

namespace Application.Interfaces;

public interface IProductRepository
{
    Task<Product> Add(Product product, CancellationToken cancellationToken = default);
    Task<List<Product>> GetAll(CancellationToken cancellationToken = default);
    Task<Product?> GetById(Guid id, CancellationToken cancellationToken = default);
    Task<IEnumerable<Product>> GetPaged(int pageNumber, int pageSize, CancellationToken cancellationToken = default);
    Task Delete(Product product, CancellationToken cancellationToken = default);
    Task UpdateDescription(Guid id, string? description, CancellationToken cancellationToken = default);
    Task DeleteById(Guid id, CancellationToken cancellationToken = default);
}