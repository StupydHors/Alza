using Application.Interfaces;
using Application.Products.Queries;
using Domain.Entities;
using Moq;

namespace Alza.Application.Tests.Products.Queries;

public class GetProductByIdQueryHandlerTests
{
    private readonly Mock<IProductRepository> repositoryMock;
    private readonly GetProductByIdQueryHandler handler;

    public GetProductByIdQueryHandlerTests()
    {
        repositoryMock = new Mock<IProductRepository>();
        handler = new GetProductByIdQueryHandler(repositoryMock.Object);
    }

    [Fact]
    public async Task Handle_WithExistingProduct_ReturnsProductDto()
    {
        // Arrange
        var productId = Guid.NewGuid();
        var product = new Product("Test Product", "test.jpg", 9.99m, "Test Description");

        repositoryMock.Setup(r => r.GetById(productId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(product);

        // Act
        var result = await handler.Handle(new GetProductByIdQuery(productId), CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(product.Name, result.Value.Name);
    }

    [Fact]
    public async Task Handle_WithNonExistingProduct_ReturnsError()
    {
        // Arrange
        var productId = Guid.NewGuid();

        repositoryMock.Setup(r => r.GetById(productId, It.IsAny<CancellationToken>()))
            .ReturnsAsync((Product?)null);

        // Act
        var result = await handler.Handle(new GetProductByIdQuery(productId), CancellationToken.None);

        // Assert
        Assert.True(result.Errors[0].Message == "Product not found");
    }
}