namespace Domain.Entities;

public record Product
{
    public Guid Id { get; private set; }
    public string Name { get; private set; }
    public string ImgUri { get; private set; }
    public decimal Price { get; private set; }
    public string? Description { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime? LastModifiedAt { get; private set; }

    private Product() { } // Private constructor for EF Core

    public Product(string name, string imgUri, decimal price, string? description = null)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Name is required", nameof(name));

        if (string.IsNullOrWhiteSpace(imgUri))
            throw new ArgumentException("Image URI is required", nameof(imgUri));

        if (price < 0)
            throw new ArgumentException("Price cannot be negative", nameof(price));

        Id = Guid.NewGuid();
        Name = name;
        ImgUri = imgUri;
        Price = price;
        Description = description;
        CreatedAt = DateTime.UtcNow;
    }

    public void UpdateDescription(string? newDescription)
    {
        Description = newDescription;
        LastModifiedAt = DateTime.UtcNow;
    }
}