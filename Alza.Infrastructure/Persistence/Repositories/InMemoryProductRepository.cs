using Application.Interfaces;
using Domain.Entities;

namespace Infrastructure.Persistence.Repositories;

public class InMemoryProductRepository : IProductRepository
{
    private readonly List<Product> productList = new();

    public async Task<Product> Add(Product entity, CancellationToken cancellationToken = default)
    {
        if (productList.Any(p => p.Id == entity.Id))
        {
            throw new ApplicationException("Product with the same id already exists");
        }

        productList.Add(entity);

        return await Task.FromResult(entity);
    }

    public async Task<List<Product>> GetAll(CancellationToken cancellationToken = default)
    {
        return await Task.FromResult(productList.ToList());
    }

    public async Task<Product?> GetById(Guid id, CancellationToken cancellationToken = default)
    {
        return await Task.FromResult(productList.FirstOrDefault(p => p.Id == id));
    }

    public async Task<IEnumerable<Product>> GetPaged(int pageNumber, int pageSize, CancellationToken cancellationToken = default)
    {
        var items = productList
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .OrderBy(p => p.CreatedAt)
            .ToList();

        return await Task.FromResult(items);
    }

    public async Task Delete(Product product, CancellationToken cancellationToken = default)
    {
        productList.Remove(product);
        await Task.CompletedTask;
    }

    public async Task UpdateDescription(Guid id, string? description, CancellationToken cancellationToken = default)
    {
        var product = productList.FirstOrDefault(p => p.Id == id);

        if (product != null)
        {
            product.UpdateDescription(description);
        }

        await Task.CompletedTask;
    }

    public async Task DeleteById(Guid id, CancellationToken cancellationToken = default)
    {
        productList.RemoveAll(p => p.Id == id);
        await Task.CompletedTask;
    }
}