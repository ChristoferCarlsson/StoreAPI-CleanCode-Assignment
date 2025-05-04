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
            return Ok(result);
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _mediator.Send(new GetAllCategoriesQuery());
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _mediator.Send(new GetCategoryByIdQuery(id));
            return Ok(result);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateCategoryCommand command)
        {
            if (id != command.Id)
            {
                return BadRequest("Category ID mismatch.");
            }

            var result = await _mediator.Send(command);
            return Ok(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var command = new DeleteCategoryCommand(id);
            await _mediator.Send(command);
            return NoContent();  // Successfully deleted, return 204 No Content
        }
    }
}
