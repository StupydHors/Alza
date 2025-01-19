using Application.Common.Models;
using Application.Interfaces;
using Domain.Entities;
using FluentResults;
using MediatR;

namespace Application.Products.Commands;

public record CreateProductCommand(Product Product) : IRequest<Result<ProductDto>>;

public class CreateProductCommandHandler(IProductRepository productRepository)
    : IRequestHandler<CreateProductCommand, Result<ProductDto>>
{
    public async Task<Result<ProductDto>> Handle(CreateProductCommand request, CancellationToken cancellationToken)
    {
        var product = await productRepository.Add(request.Product, cancellationToken);

        return Result.Ok(new ProductDto()
        {
            Id = product.Id,
            Name = product.Name,
            ImgUri = product.ImgUri,
            Price = product.Price,
            Description = product.Description
        });
    }
}