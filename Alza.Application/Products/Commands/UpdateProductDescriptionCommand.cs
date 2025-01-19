using Application.Interfaces;
using FluentResults;
using MediatR;

namespace Application.Products.Commands;

public record UpdateProductDescriptionCommand(Guid Id, string? Description) : IRequest<Result>;

public class UpdateProductDescriptionCommandHandler(IProductRepository productRepository)
    : IRequestHandler<UpdateProductDescriptionCommand, Result>
{
    public async Task<Result> Handle(UpdateProductDescriptionCommand request, CancellationToken cancellationToken)
    {
        var product = await productRepository.GetById(request.Id, cancellationToken);

        if (product == null)
        {
            return Result.Fail("Product not found");
        }

        await productRepository.UpdateDescription(request.Id, request.Description, cancellationToken);

        return Result.Ok();
    }
}