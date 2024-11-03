using Microsoft.AspNetCore.Mvc;
using OnlineShopping.CartService.Domain.Entities;
using OnlineShopping.CatalogService.Application.Categories.Commands;
using OnlineShopping.CatalogService.Application.Categories.Queries;

namespace OnlineShopping.CatalogService.API
{
    public class CategoryController : CatalogControllerBase
    {
        private readonly ILogger<CategoryController> _logger;

        public CategoryController(
            ILogger<CategoryController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<List<Category>>> GetCategories()
        {
            var categories = await Mediator.Send(new GetCategoriesQuery());

            return Ok(categories);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<Category>> GetCategory([FromRoute] int id)
        {
            var category = await Mediator.Send(new GetCategoryQuery(id));

            return Ok(category);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<ActionResult<int>> CreateCategory([FromBody] CreateCategoryCommand command)
        {
            int id = await Mediator.Send(command);

            return CreatedAtAction(nameof(GetCategory), new { id });
        }

        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<Category>> UpdateCategory([FromRoute] int id, [FromBody] UpdateCategoryCommand command)
        {
            if (id != command.Id)
            {
                return ValidationProblem($"Route id {id} does not match request body id {command.Id}");
            }

            await Mediator.Send(command);

            return NoContent();
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<ActionResult<Category>> DeleteCategory([FromRoute] int id)
        {
            await Mediator.Send(new DeleteCategoryCommand(id));

            return NoContent();
        }
    }
}