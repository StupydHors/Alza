using Application.Interfaces;
using FluentResults;
using MediatR;

namespace Application.Products.Commands;

public record DeleteProductCommand(Guid Id) : IRequest<Result>;

public class DeleteProductCommandHandler(IProductRepository productRepository)
    : IRequestHandler<DeleteProductCommand, Result>
{
    public async Task<Result> Handle(DeleteProductCommand request, CancellationToken cancellationToken)
    {
        var product = await productRepository.GetById(request.Id, cancellationToken);

        if (product == null)
        {
            return Result.Fail("Product not found");
        }

        await productRepository.Delete(product, cancellationToken);

        return Result.Ok();
    }
}