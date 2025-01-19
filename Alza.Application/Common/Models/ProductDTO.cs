namespace Application.Common.Models;

public record ProductDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string ImgUri { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public string? Description { get; set; }
}
