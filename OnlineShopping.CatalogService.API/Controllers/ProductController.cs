﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnlineShopping.CatalogService.Application.Categories.DTOs;
using OnlineShopping.CatalogService.Application.Common.Models;
using OnlineShopping.CatalogService.Application.Products.Commands;
using OnlineShopping.CatalogService.Application.Products.Queries;
using OnlineShopping.Shared.Auth;

namespace OnlineShopping.CatalogService.API.Controllers;

public class ProductController : CatalogControllerBase
{
    [HttpGet(Name = nameof(GetProducts))]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<List<ProductDTO>>> GetProducts([FromQuery] SieveInputModel sieveInput, [FromQuery] PaginationModel pagination)
    {
        var categories = await Mediator.Send(new GetProductsQuery(sieveInput, pagination));

        return Ok(categories);
    }

    [HttpGet("{id}", Name = nameof(GetProduct))]
    [Authorize]
    [ResponseCache(CacheProfileName = "Product")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<ProductDTO>> GetProduct([FromRoute] int id)
    {
        var product = await Mediator.Send(new GetProductQuery(id));

        return Ok(product);
    }

    [HttpPost(Name = nameof(CreateProduct))]
    [Authorize(Roles = AuthorizationConstants.Roles.ManagerRole)]
    [ProducesResponseType(StatusCodes.Status201Created)]
    public async Task<ActionResult> CreateProduct([FromBody] CreateProductCommand command)
    {
        int id = await Mediator.Send(command);

        return CreatedAtAction(nameof(GetProduct), new { id });
    }

    [HttpPut("{id}", Name = nameof(UpdateProduct))]
    [Authorize(Roles = AuthorizationConstants.Roles.ManagerRole)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> UpdateProduct([FromRoute] int id, [FromBody] UpdateProductCommand command)
    {
        if (id != command.Id)
        {
            return ValidationProblem($"Route id {id} does not match request body id {command.Id}");
        }

        await Mediator.Send(command);

        return NoContent();
    }

    [HttpDelete("{id}", Name = nameof(DeleteProduct))]
    [Authorize(Roles = AuthorizationConstants.Roles.ManagerRole)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> DeleteProduct([FromRoute] int id)
    {
        await Mediator.Send(new DeleteProductCommand(id));

        return NoContent();
    }
}