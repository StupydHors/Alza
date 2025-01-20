using Application.Interfaces;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Repositories;

public class ProductRepository(ApplicationDbContext dbContext) : IProductRepository
{
    public async Task<Product> Add(Product product, CancellationToken cancellationToken = default)
    {
        var result = await dbContext.Products.AddAsync(product, cancellationToken);
        await dbContext.SaveChangesAsync(cancellationToken);

        return result.Entity;
    }

    public async Task<List<Product>> GetAll(CancellationToken cancellationToken = default)
    {
        return await dbContext.Products.ToListAsync(cancellationToken);
    }

    public async Task<Product?> GetById(Guid id, CancellationToken cancellationToken = default)
    {
        var product = await dbContext.Products.FindAsync([id], cancellationToken);

        return product;
    }

    public async Task<IEnumerable<Product>> GetPaged(int pageNumber, int pageSize, CancellationToken cancellationToken = default)
    {
        if (pageNumber < 0 || pageSize < 0)
        {
            throw new ArgumentException("Page number and page size must be greater than 0");
        }

        var items = await dbContext.Products
            .Skip(pageNumber * pageSize)
            .Take(pageSize)
            .OrderBy(p => p.CreatedAt)
            .ToListAsync(cancellationToken);

        return items;
    }

    public async Task Delete(Product product, CancellationToken cancellationToken = default)
    {
        dbContext.Products.Remove(product);
        await dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteById(Guid id, CancellationToken cancellationToken = default)
    {
        var product = await GetById(id, cancellationToken);

        if (product != null)
        {
            await Delete(product, cancellationToken);
        }
    }

    public async Task UpdateDescription(Guid id, string? description, CancellationToken cancellationToken = default)
    {
        var product = await GetById(id, cancellationToken);

        if (product != null)
        {
            product.UpdateDescription(description);
            await dbContext.SaveChangesAsync(cancellationToken);
        }
    }
}