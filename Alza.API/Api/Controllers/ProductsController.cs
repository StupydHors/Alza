using Application.Common.Models;
using Application.Products.Commands;
using Application.Products.Queries;
using Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Api.Api.Controllers;

/// <summary>
/// Products API
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class ProductsController(IMediator mediator) : ControllerBase
{
    /// <summary>
    /// Get all products
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <returns>All the products</returns>
    [HttpGet("v1")]
    public async Task<ActionResult<IEnumerable<ProductDto>>> GetAll(CancellationToken cancellationToken)
    {
        var result = await mediator.Send(new GetProductsQuery(0, int.MaxValue), cancellationToken);

        if (result.IsFailed) return StatusCode(StatusCodes.Status500InternalServerError);

        return Ok(result.Value.Items);
    }

    /// <summary>
    /// Gets a paginated list of products
    /// </summary>
    /// <param name="pageNumber">The page number (0-based)</param>
    /// <param name="pageSize">The number of items per page</param>
    /// <param name="cancellationToken"></param>
    /// <returns>A paginated list of products</returns>
    [HttpGet("v2")]
    public async Task<ActionResult<PaginatedList<ProductDto>>> GetPaged(
        [FromQuery] int pageNumber = 0,
        [FromQuery] int pageSize = 10,
        CancellationToken cancellationToken = default)
    {
        var result = await mediator.Send(new GetProductsQuery(pageNumber, pageSize), cancellationToken);

        if (!result.IsFailed) return Ok(result.Value);

        if (result.Errors.Exists(x => x.Message == "Page number and page size must not be less than 0"))
        {
            return BadRequest("Page number and page size must not be less than 0");
        }

        return StatusCode(StatusCodes.Status500InternalServerError);
    }

    /// <summary>
    /// Gets a product by id
    /// </summary>
    /// <param name="id">The id of the product</param>
    /// <param name="cancellationToken"></param>
    /// <returns>One product with the specified id</returns>
    [HttpGet("{id}")]
    public async Task<ActionResult<ProductDto>> GetById(Guid id, CancellationToken cancellationToken)
    {
        var result = await mediator.Send(new GetProductByIdQuery(id), cancellationToken);

        if (!result.IsFailed) return Ok(result.Value);

        if (result.Errors.Exists(x => x.Message == "Product not found"))
        {
            return NotFound();
        }

        return StatusCode(StatusCodes.Status500InternalServerError);
    }

    /// <summary>
    /// Updates the description of a product
    /// </summary>
    /// <param name="id">The id of the product</param>
    /// <param name="description">New description</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpPatch("{id}/description")]
    public async Task<IActionResult> UpdateDescription(Guid id, [FromBody] string? description, CancellationToken cancellationToken)
    {
        var result = await mediator.Send(new UpdateProductDescriptionCommand(id, description), cancellationToken);

        if (result.Errors.Exists(x => x.Message == "Product not found"))
        {
            return NotFound();
        }

        return NoContent();
    }

    /// <summary>
    /// Deletes a product
    /// </summary>
    /// <param name="id">The id of the product</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken)
    {
        var result = await mediator.Send(new DeleteProductCommand(id), cancellationToken);

        if (result.Errors.Exists(x => x.Message == "Product not found"))
        {
            return NotFound();
        }

        return NoContent();
    }

    /// <summary>
    /// Creates a new product
    /// </summary>
    /// <param name="product">The product to create</param>
    /// <param name="cancellationToken"></param>
    /// <returns>The created product</returns>
    [HttpPut]
    public async Task<ActionResult<ProductDto>> Insert([FromBody] Product product, CancellationToken cancellationToken)
    {
        var result = await mediator.Send(new CreateProductCommand(product), cancellationToken);

        return Ok(result.Value);
    }
}