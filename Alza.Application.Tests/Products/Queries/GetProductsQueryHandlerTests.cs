using Application.Common.Models;
using Application.Interfaces;
using Application.Products.Queries;
using Domain.Entities;
using FluentResults;
using Moq;

namespace Alza.Application.Tests.Products.Queries;

public class GetProductsQueryHandlerTests
{
    private readonly Mock<IProductRepository> repositoryMock;
    private readonly GetProductsQueryHandler handler;

    public GetProductsQueryHandlerTests()
    {
        repositoryMock = new Mock<IProductRepository>();
        handler = new GetProductsQueryHandler(repositoryMock.Object);
    }

    [Fact]
    public async Task Handle_WithExistingProduct_ReturnsProductDto()
    {
        // Arrange
        var product = new Product("Test Product", "test.jpg", 9.99m, "Test Description");

        repositoryMock.Setup(r => r.GetPaged(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new List<Product> { product });

        // Act
        var result = await handler.Handle(new GetProductsQuery(1, 10), CancellationToken.None);

        // Assert
        Assert.IsType<Result<PaginatedList<ProductDto>>>(result);
        Assert.Single(result.Value.Items);
        Assert.Equal(product.Name, result.Value.Items[0].Name);
    }

    [Fact]
    public async Task Handle_WithInvalidPageNumber_ReturnsError()
    {
        // Arrange
        // Act
        var result = await handler.Handle(new GetProductsQuery(-1, 10), CancellationToken.None);

        // Assert
        Assert.IsType<Result<PaginatedList<ProductDto>>>(result);
        Assert.Equal("Page number and page size must not be less than 0", result.Errors[0].Message);
    }

    [Fact]
    public async Task Handle_WithInvalidPageSize_ReturnsError()
    {
        // Arrange
        // Act
        var result = await handler.Handle(new GetProductsQuery(1, -1), CancellationToken.None);

        // Assert
        Assert.IsType<Result<PaginatedList<ProductDto>>>(result);
        Assert.Equal("Page number and page size must not be less than 0", result.Errors[0].Message);
    }
}