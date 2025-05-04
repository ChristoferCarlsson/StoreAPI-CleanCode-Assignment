using Application.Products.Commands.CreateProduct;
using Application.Products.Commands.UpdateProduct;
using Application.Products.Queries.GetAllProducts;
using MediatR;
using Microsoft.AspNetCore.Mvc;

using Application.Products.Queries.GetAllProducts;
using Application.Products.Queries.GetProductById;
using Application.Products.Commands.DeleteProduct;
using Microsoft.AspNetCore.Authorization;

namespace API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductsController : ControllerBase
{
    private readonly IMediator _mediator;

    public ProductsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateProductCommand command)
    {
        try
        {
            var result = await _mediator.Send(command);
            return CreatedAtAction(nameof(Create), new { id = result.Id }, result); // Return the created product.
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message); // Return error if the product already exists.
        }
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, UpdateProductCommand command)
    {
        if (id != command.Id)
            return BadRequest("Product ID mismatch.");

        var result = await _mediator.Send(command);
        return Ok(result);
    }

    [Authorize]
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var result = await _mediator.Send(new GetAllProductsQuery());

        if (!result.Success)
        {
            return NotFound(result.Message);  // Return 404 with message if the result is not successful
        }

        return Ok(result.Data);  // Return the list of products if successful
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var result = await _mediator.Send(new GetProductByIdQuery(id));

        if (!result.Success)
        {
            return NotFound(result.Message);
        }

        return Ok(result.Data);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var result = await _mediator.Send(new DeleteProductCommand(id));

        if (!result.Success)
            return NotFound(result.Message);

        return NoContent();
    }

}
