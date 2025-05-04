using Application.Categories.Commands.CreateCategory;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Application.DTOs;
using Application.Categories.Queries.GetAllCategories;
using Application.Categories.Queries.GetCategoryById;
using Application.Categories.Commands.UpdateCategory;
using Application.Categories.Commands.DeleteCategory;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CategoriesController : ControllerBase
    {
        private readonly IMediator _mediator;

        public CategoriesController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateCategoryCommand command)
        {
            var result = await _mediator.Send(command);

            // Check if the result was successful
            if (result.Success)
            {
                return CreatedAtAction(nameof(Create), new { id = result.Data.Id }, result.Data); // Return the created category
            }

            // If there was an error, return the failure message
            return BadRequest(result.Message);
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _mediator.Send(new GetAllCategoriesQuery());

            // Check if the result is not null and return an appropriate response.
            if (result.Success)
            {
                return Ok(result);  // Return the result with a 200 OK status.
            }
            else
            {
                return NotFound(result.Message);  // Return a 404 Not Found with the failure message.
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _mediator.Send(new GetCategoryByIdQuery(id));

            if (result.Success)
            {
                return Ok(result);  // Return the result with a 200 OK status.
            }
            else
            {
                return NotFound(result.Message);  // Return 404 Not Found with the failure message.
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateCategoryCommand command)
        {
            if (id != command.Id)
            {
                return BadRequest("Category ID mismatch.");
            }

            var result = await _mediator.Send(command);

            if (result.Success)
            {
                return Ok(result);  // Return the result with a 200 OK status.
            }
            else
            {
                return NotFound(result.Message);  // Return 404 Not Found with the failure message.
            }
        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var command = new DeleteCategoryCommand(id);
            var result = await _mediator.Send(command);

            if (result.Success)
            {
                return NoContent();  // Successfully deleted, return 204 No Content.
            }
            else
            {
                return NotFound(result.Message);  // Return 404 Not Found if deletion fails.
            }
        }

    }
}
