using Application.Common.Models;
using Application.Products.Commands;
using Application.Products.Queries;
using Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using MediatR;

namespace Alza.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductsController : ControllerBase
{
    private readonly IMediator _mediator;

    public ProductsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("v1")]
    public async Task<ActionResult<IEnumerable<ProductDto>>> GetAll(CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetProductsQuery(0, int.MaxValue), cancellationToken);

        if (result.IsFailed) return StatusCode(StatusCodes.Status500InternalServerError);

        return Ok(result.Value.Items);
    }

    [HttpGet("v2")]
    public async Task<ActionResult<PaginatedList<ProductDto>>> GetPaged(
        [FromQuery] int pageNumber = 0,
        [FromQuery] int pageSize = 10,
        CancellationToken cancellationToken = default)
    {
        var result = await _mediator.Send(new GetProductsQuery(pageNumber, pageSize), cancellationToken);

        if (!result.IsFailed) return Ok(result.Value);

        if (result.Errors.Exists(x => x.Message == "Page number and page size must not be less than 0"))
        {
            return BadRequest("Page number and page size must not be less than 0");
        }

        return StatusCode(StatusCodes.Status500InternalServerError);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ProductDto>> GetById(Guid id, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetProductByIdQuery(id), cancellationToken);

        if (!result.IsFailed) return Ok(result.Value);

        if (result.Errors.Exists(x => x.Message == "Product not found"))
        {
            return NotFound();
        }

        return StatusCode(StatusCodes.Status500InternalServerError);
    }

    [HttpPatch("{id}/description")]
    public async Task<IActionResult> UpdateDescription(Guid id, [FromBody] string? description, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new UpdateProductDescriptionCommand(id, description), cancellationToken);

        if (result.Errors.Exists(x => x.Message == "Product not found"))
        {
            return NotFound();
        }

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new DeleteProductCommand(id), cancellationToken);

        if (result.Errors.Exists(x => x.Message == "Product not found"))
        {
            return NotFound();
        }

        return NoContent();
    }

    [HttpPut]
    public async Task<ActionResult<ProductDto>> Insert([FromBody] Product product, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new CreateProductCommand(product), cancellationToken);

        return Ok(result.Value);
    }
}