using Domain.Entities;

namespace Application.Interfaces;

public interface IProductRepository
{
    Task Add(Product product, CancellationToken cancellationToken = default);
    Task<List<Product>> GetAll(CancellationToken cancellationToken = default);
}