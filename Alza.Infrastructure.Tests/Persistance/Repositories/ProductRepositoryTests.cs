using Domain.Entities;
using Infrastructure.Persistence;
using Infrastructure.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Alza.Infrastructure.Tests.Persistance.Repositories;

public class ProductRepositoryTests : IDisposable
{
    private readonly ApplicationDbContext context;
    private readonly ProductRepository repository;

    public ProductRepositoryTests()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        context = new ApplicationDbContext(options);
        repository = new ProductRepository(context);
    }

    [Fact]
    public async Task Add_ReturnsProduct_WhenInserted()
    {
        // Arrange
        var product = new Product("Test Product", "test.jpg", 9.99m, "Test Description");

        // Act
        var result = await repository.Add(product);

        // Assert
        Assert.Equal(1, context.Products.Count());
        Assert.Equal(result, product);
    }

    [Fact]
    public async Task GetByIdAsync_ReturnsProduct_WhenProductExists()
    {
        // Arrange
        var product = new Product("Test Product", "test.jpg", 9.99m, "Test Description");
        await context.Products.AddAsync(product);
        await context.SaveChangesAsync();

        // Act
        var result = await repository.GetById(product.Id);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(product.Name, result.Name);
    }

    [Fact]
    public async Task GetPagedAsync_ReturnsCorrectPage()
    {
        // Arrange
        var products = new List<Product>
        {
            new("Product 1", "test1.jpg", 9.99m),
            new("Product 2", "test2.jpg", 19.99m),
            new("Product 3", "test3.jpg", 29.99m),
            new("Product 4", "test4.jpg", 39.99m),
            new("Product 5", "test5.jpg", 49.99m)
        };

        await context.Products.AddRangeAsync(products);
        await context.SaveChangesAsync();

        // Act
        var items1 = await repository.GetPaged(0, 3);
        var items2 = await repository.GetPaged(1, 3);

        // Assert
        Assert.Equal(3, items1.Count());
        Assert.Equal(2, items2.Count());
    }

    [Fact]
    public async Task DeleteById_DeletesProduct()
    {
        // Arrange
        var products = new List<Product>
        {
            new("Product 1", "test1.jpg", 9.99m),
            new("Product 2", "test2.jpg", 19.99m),
            new("Product 3", "test3.jpg", 29.99m)
        };

        await context.Products.AddRangeAsync(products);
        await context.SaveChangesAsync();

        // Act
        await repository.DeleteById(products[1].Id);

        // Assert
        Assert.Equal(2, context.Products.Count());
    }

    [Fact]
    public async Task Delete_DeletesProduct()
    {
        // Arrange
        var products = new List<Product>
        {
            new("Product 1", "test1.jpg", 9.99m),
            new("Product 2", "test2.jpg", 19.99m),
            new("Product 3", "test3.jpg", 29.99m)
        };

        await context.Products.AddRangeAsync(products);
        await context.SaveChangesAsync();

        // Act
        await repository.Delete(products[1]);

        // Assert
        Assert.Equal(2, context.Products.Count());
    }

    [Fact]
    public async Task UpdateDescripton_ChangesDescription()
    {
        // Arrange
        var products = new List<Product>
        {
            new("Product 1", "test1.jpg", 9.99m),
            new("Product 2", "test2.jpg", 19.99m),
            new("Product 3", "test3.jpg", 29.99m)
        };

        await context.Products.AddRangeAsync(products);
        await context.SaveChangesAsync();

        // Act
        await repository.UpdateDescription(products[1].Id, "New Description");

        // Assert
        Assert.Equal("New Description", context.Products.Single(p => p.Id == products[1].Id).Description);
    }

    public void Dispose()
    {
        context.Database.EnsureDeleted();
        context.Dispose();
    }
}