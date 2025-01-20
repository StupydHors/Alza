using Api.Api.Controllers;
using Application.Common.Models;
using Application.Products.Commands;
using Application.Products.Queries;
using Domain.Entities;
using FluentResults;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace Alza.Api.Tests;

public class ProductsControllerTests
{
    private readonly Mock<IMediator> mediatorMock;
    private readonly ProductsController controller;

    public ProductsControllerTests()
    {
        mediatorMock = new Mock<IMediator>();
        controller = new ProductsController(mediatorMock.Object);
    }

    [Fact]
    public async Task GetPaged_ReturnsOkResult_WithPaginatedList()
    {
        // Arrange
        var expectedResponse = new PaginatedList<ProductDto>(
            [new() { Id = Guid.NewGuid(), Name = "Test Product" }],
            1,
            10
            );

        mediatorMock.Setup(m => m.Send(It.IsAny<GetProductsQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(expectedResponse);

        // Act
        var result = await controller.GetPaged(1, 10);

        // Assert
        mediatorMock.Verify(m => m.Send(It.Is<GetProductsQuery>(q => q.PageNumber == 1 && q.PageSize == 10), It.IsAny<CancellationToken>()));
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var returnValue = Assert.IsType<PaginatedList<ProductDto>>(okResult.Value);
        Assert.Single(returnValue.Items);
    }

    [Fact]
    public async Task GetById_WithValidId_ReturnsOkResult()
    {
        // Arrange
        var productId = Guid.NewGuid();
        var expectedProduct = Result.Ok(new ProductDto { Id = productId, Name = "Test Product" });

        mediatorMock.Setup(m => m.Send(It.Is<GetProductByIdQuery>(q => q.Id == productId), It.IsAny<CancellationToken>()))
            .ReturnsAsync(expectedProduct);

        // Act
        var result = await controller.GetById(productId, CancellationToken.None);

        // Assert
        mediatorMock.Verify(m => m.Send(It.Is<GetProductByIdQuery>(q => q.Id == productId), It.IsAny<CancellationToken>()));
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var returnValue = Assert.IsType<ProductDto>(okResult.Value);
        Assert.Equal(productId, returnValue.Id);
    }

    [Fact]
    public async Task UpdateDescription_WithValidData_ReturnsNoContent()
    {
        // Arrange
        var productId = Guid.NewGuid();
        var description = "Updated description";

        mediatorMock.Setup(m => m.Send(
            It.Is<UpdateProductDescriptionCommand>(q => q.Id == productId), It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result.Ok());

        // Act
        var result = await controller.UpdateDescription(productId, description, CancellationToken.None);

        // Assert
        Assert.IsType<NoContentResult>(result);
        mediatorMock.Verify(m => m.Send(It.Is<UpdateProductDescriptionCommand>(
            c => c.Id == productId && c.Description == description), It.IsAny<CancellationToken>()));
    }

    [Fact]
    public async Task Delete_WithValidId_ReturnsNoContent()
    {
        // Arrange
        var productId = Guid.NewGuid();

        mediatorMock.Setup(m => m.Send(
            It.Is<DeleteProductCommand>(q => q.Id == productId), It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result.Ok());

        // Act
        var result = await controller.Delete(productId, CancellationToken.None);

        // Assert
        mediatorMock.Verify(m => m.Send(It.Is<DeleteProductCommand>(q => q.Id == productId), It.IsAny<CancellationToken>()));
        Assert.IsType<NoContentResult>(result);
    }

    [Fact]
    public async Task Insert_WithValidData_ReturnsCreatedResult()
    {
        // Arrange
        var product = new Product("Test Product", "some.url", 10m, "description text");

        var productDto = new ProductDto
        {
            Name = product.Name,
            ImgUri = product.ImgUri,
            Price = product.Price,
            Description = product.Description,
            Id = product.Id
        };

        mediatorMock.Setup(m => m.Send(
            It.Is<CreateProductCommand>(q => q.Product == product), It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result.Ok(productDto));

        // Act
        var result = await controller.Insert(product, CancellationToken.None);

        // Assert
        mediatorMock.Verify(
            m => m.Send(It.Is<CreateProductCommand>(q => q.Product == product), It.IsAny<CancellationToken>()));
        var createdResult = Assert.IsType<OkObjectResult>(result.Result);
        var returnValue = Assert.IsType<ProductDto>(createdResult.Value);
        Assert.Equal(product.Id, returnValue.Id);
    }
}
