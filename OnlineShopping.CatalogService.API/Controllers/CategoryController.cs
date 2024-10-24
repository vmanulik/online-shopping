﻿using Microsoft.AspNetCore.Mvc;
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

        [HttpGet()]
        public async Task<ActionResult<List<Category>>> GetCategories()
        {
            var categories = await Mediator.Send(new GetCategoriesQuery());

            return Ok(categories);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Category>> GetCategory([FromRoute] int id)
        {
            var category = await Mediator.Send(new GetCategoryQuery(id));

            return Ok(category);
        }

        [HttpPost()]
        public async Task<ActionResult<int>> CreateCategory([FromBody] CreateCategoryCommand command)
        {
            int id = await Mediator.Send(command);

            return Ok(id);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<Category>> UpdateCategory([FromRoute] int id, [FromBody] UpdateCategoryCommand command)
        {
            if (id != command.Id)
            {
                return BadRequest($"Route id {id} does not match request body id {command.Id}");
            }

            await Mediator.Send(command);

            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<Category>> DeleteCategory([FromRoute] int id)
        {
            await Mediator.Send(new DeleteCategoryCommand(id));

            return Ok();
        }
    }
}