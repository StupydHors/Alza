using Application.Interfaces;
using Application.Products.Commands;
using Domain.Entities;
using Moq;

namespace Alza.Application.Tests.Products.Commands;

public class CreateProductCommandTests
{
    private readonly Mock<IProductRepository> _repositoryMock;
    private readonly CreateProductCommandHandler _handler;

    public CreateProductCommandTests()
    {
        _repositoryMock = new Mock<IProductRepository>();
        _handler = new CreateProductCommandHandler(_repositoryMock.Object);
    }

    [Fact]
    public async Task Handle_WithValidData_CreatesProduct()
    {
        // Arrange
        var product = new Product("Test Product", "test.jpg", 9.99m, "Test Description");

        _repositoryMock.Setup(r => r.Add(It.IsAny<Product>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(product);

        // Act
        var result = await _handler.Handle(new CreateProductCommand(product), CancellationToken.None);

        // Assert
        Assert.Equal(product.Id, result.Value.Id);
        _repositoryMock.Verify(r => r.Add(It.IsAny<Product>(), It.IsAny<CancellationToken>()), Times.Once);
    }
}