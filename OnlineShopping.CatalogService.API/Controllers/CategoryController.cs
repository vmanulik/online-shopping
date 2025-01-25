using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnlineShopping.CatalogService.Application.Categories.Commands;
using OnlineShopping.CatalogService.Application.Categories.DTOs;
using OnlineShopping.CatalogService.Application.Categories.Queries;
using OnlineShopping.CatalogService.Application.Common.Models;
using OnlineShopping.Shared.Auth;

namespace OnlineShopping.CatalogService.API.Controllers;

public class CategoryController : CatalogControllerBase
{
    [HttpGet]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<List<CategoryDTO>>> GetCategories([FromQuery] SieveInputModel sieveInput, [FromQuery] PaginationModel pagination)
    {
        var categories = await Mediator.Send(new GetCategoriesQuery(sieveInput, pagination));

        return Ok(categories);
    }

    [HttpGet("{id}")]
    [Authorize]
    [ResponseCache(CacheProfileName = "Category")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<CategoryDTO>> GetCategory([FromRoute] int id)
    {
        var category = await Mediator.Send(new GetCategoryQuery(id));

        return Ok(category);
    }

    [HttpPost]
    [Authorize(Roles = AuthorizationConstants.Roles.ManagerRole)]
    [ProducesResponseType(StatusCodes.Status201Created)]
    public async Task<IActionResult> CreateCategory([FromBody] CreateCategoryCommand command)
    {
        int id = await Mediator.Send(command);

        return CreatedAtAction(nameof(GetCategory), new { id });
    }

    [HttpPut("{id}")]
    [Authorize(Roles = AuthorizationConstants.Roles.ManagerRole)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> UpdateCategory([FromRoute] int id, [FromBody] UpdateCategoryCommand command)
    {
        if (id != command.Id)
        {
            return ValidationProblem($"Route id {id} does not match request body id {command.Id}");
        }

        await Mediator.Send(command);

        return NoContent();
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = AuthorizationConstants.Roles.ManagerRole)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> DeleteCategory([FromRoute] int id)
    {
        await Mediator.Send(new DeleteCategoryCommand(id));

        return NoContent();
    }
}