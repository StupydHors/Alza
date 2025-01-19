using Application.Common.Models;
using Application.Interfaces;
using FluentResults;
using MediatR;

namespace Application.Products.Queries;

public record GetProductsQuery(int PageNumber = 1, int PageSize = 10) : IRequest<Result<PaginatedList<ProductDto>>>;

public class GetProductsQueryHandler : IRequestHandler<GetProductsQuery, Result<PaginatedList<ProductDto>>>
{
    private readonly IProductRepository _repository;

    public GetProductsQueryHandler(IProductRepository repository)
    {
        _repository = repository;
    }

    public async Task<Result<PaginatedList<ProductDto>>> Handle(GetProductsQuery request, CancellationToken cancellationToken)
    {
        if (request.PageNumber < 0 || request.PageSize < 0)
        {
            return Result.Fail("Page number and page size must not be less than 0");
        }

        try
        {
            var products = await _repository.GetPaged(request.PageNumber, request.PageSize, cancellationToken);

            var dtos = products.Select(p => new ProductDto
            {
                Id = p.Id,
                Name = p.Name,
                ImgUri = p.ImgUri,
                Price = p.Price,
                Description = p.Description
            });

            return Result.Ok(new PaginatedList<ProductDto>(dtos.ToList(), request.PageNumber, request.PageSize));
        }
        catch (Exception e)
        {
            return Result.Fail("An error occurred while retrieving products.");
        }
    }
}