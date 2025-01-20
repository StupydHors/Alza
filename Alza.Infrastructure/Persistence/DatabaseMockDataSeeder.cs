using Application.Interfaces;
using Bogus;
using Domain.Entities;

namespace Infrastructure.Persistence;

public class DatabaseMockDataSeeder(IProductRepository productRepository)
{
    public async Task SeedAsync()
    {
        // Only seed if the database is empty
        var allProducts = await productRepository.GetAll();
        if (allProducts.Any())
        {
            return;
        }

        var productGenerator = GetProductGenerator();
        var products = productGenerator.Generate(100);

        foreach (var product in products)
        {
            await productRepository.Add(product);
        }
    }

    private static Faker<Product> GetProductGenerator()
    {
        return new Faker<Product>()
            .CustomInstantiator(f => new Product("a", "b", 1.0m))
            .RuleFor(e => e.Id, _ => Guid.NewGuid())
            .RuleFor(e => e.Description, f => f.Lorem.Sentences(3))
            .RuleFor(e => e.Name, f => f.Commerce.Product())
            .RuleFor(e => e.ImgUri, f => f.Internet.Avatar())
            .RuleFor(e => e.Price, f => decimal.Parse(f.Commerce.Price()))
            .RuleFor(e => e.CreatedAt, f => f.Date.Past(3, DateTime.Now).ToUniversalTime())
            .RuleFor(e => e.LastModifiedAt, f => f.Date.Past(1, DateTime.Now).ToUniversalTime());
    }
}