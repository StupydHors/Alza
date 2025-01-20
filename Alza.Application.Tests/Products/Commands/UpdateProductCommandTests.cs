using Application.Interfaces;
using Application.Products.Commands;
using Domain.Entities;
using Moq;

namespace Alza.Application.Tests.Products.Commands;

public class UpdateProductCommandTests
{
    private readonly Mock<IProductRepository> repositoryMock;
    private readonly UpdateProductDescriptionCommandHandler handler;

    public UpdateProductCommandTests()
    {
        repositoryMock = new Mock<IProductRepository>();
        handler = new UpdateProductDescriptionCommandHandler(repositoryMock.Object);
    }

    [Fact]
    public async Task Handle_WithValidData_UpdatesProduct()
    {
        // Arrange
        var productId = Guid.NewGuid();
        var existingProduct = new Product("Old Name", "old.jpg", 10.99m, "Old Description");

        repositoryMock.Setup(r => r.GetById(productId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(existingProduct);

        // Act
        await handler.Handle(new UpdateProductDescriptionCommand(productId, "Updated Description"), CancellationToken.None);

        // Assert
        repositoryMock.Verify(r => r.UpdateDescription(It.IsAny<Guid>(), It.Is<string?>(
            s => s == "Updated Description"
        ), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_WithNonExistingProduct_ThrowsNotFoundException()
    {
        // Arrange
        repositoryMock.Setup(r => r.GetById(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((Product?)null);

        // Act
        var result = await handler.Handle(
            new UpdateProductDescriptionCommand(Guid.NewGuid(), "Updated Description"),
            CancellationToken.None);

        // Assert
        repositoryMock.Verify(r => r.UpdateDescription(It.IsAny<Guid>(), It.Is<string?>(
            s => s == "Updated Description"
        ), It.IsAny<CancellationToken>()), Times.Never);
        Assert.Equal("Product not found", result.Errors[0].Message);
    }
}