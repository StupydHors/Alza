using Application.Common.Models;
using Application.Interfaces;
using FluentResults;
using MediatR;

namespace Application.Products.Queries;

public record GetProductByIdQuery(Guid Id) : IRequest<Result<ProductDto>>;

public class GetProductByIdQueryHandler(IProductRepository productRepository)
    : IRequestHandler<GetProductByIdQuery, Result<ProductDto>>
{
    private readonly IProductRepository productRepository = productRepository ?? throw new ArgumentNullException(nameof(productRepository));

    public async Task<Result<ProductDto>> Handle(GetProductByIdQuery request, CancellationToken cancellationToken)
    {
        var product = await productRepository.GetById(request.Id, cancellationToken);

        if (product == null)
        {
            return Result.Fail("Product not found");
        }

        return Result.Ok(new ProductDto
        {
            Id = product.Id,
            Name = product.Name,
            ImgUri = product.ImgUri,
            Price = product.Price,
            Description = product.Description
        });
    }
}