using Application.Interfaces;
using Application.Products.Commands;
using Domain.Entities;
using Moq;

namespace Alza.Application.Tests.Products.Commands;

public class DeleteProductCommandTests
{
    private readonly Mock<IProductRepository> repositoryMock;
    private readonly DeleteProductCommandHandler handler;

    public DeleteProductCommandTests()
    {
        repositoryMock = new Mock<IProductRepository>();
        handler = new DeleteProductCommandHandler(repositoryMock.Object);
    }

    [Fact]
    public async Task Handle_WithExistingProduct_DeletesProduct()
    {
        // Arrange
        var existingProduct = new Product("Test Product", "test.jpg", 99.99m);

        repositoryMock.Setup(r => r.GetById(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(existingProduct);

        // Act
        await handler.Handle(new DeleteProductCommand(existingProduct.Id), CancellationToken.None);

        // Assert
        repositoryMock.Verify(r => r.Delete(existingProduct, It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_WithNonExistingProduct_ThrowsNotFoundException()
    {
        // Arrange
        repositoryMock.Setup(r => r.GetById(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((Product?)null);

        // Act
        var result = await handler.Handle(new DeleteProductCommand(Guid.NewGuid()), CancellationToken.None);

        // Assert
        repositoryMock.Verify(r => r.Delete(It.IsAny<Product>(), It.IsAny<CancellationToken>()), Times.Never);
        Assert.Equal("Product not found", result.Errors[0].Message);
    }
}