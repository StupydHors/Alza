using Bogus;
using Domain.Entities;

namespace Infrastructure.Persistence;

public class DatabaseMockDataSeeder(ApplicationDbContext context)
{
    public async Task SeedAsync()
    {
        // Only seed if the database is empty
        if (context.Products.Any())
        {
            return;
        }

        var productGenerator = GetProductGenerator();
        var products = productGenerator.Generate(100);

        await context.Products.AddRangeAsync(products);
        await context.SaveChangesAsync();
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