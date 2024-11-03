using Microsoft.AspNetCore.Mvc;
using OnlineShopping.CartService.Domain.Entities;
using OnlineShopping.CatalogService.Application.Products.Commands;
using OnlineShopping.CatalogService.Application.Products.Queries;

namespace OnlineShopping.CatalogService.API
{
    public class ProductController : CatalogControllerBase
    {
        private readonly ILogger<ProductController> _logger;

        public ProductController(
            ILogger<ProductController> logger)
        {
            _logger = logger;
        }

        [HttpGet()]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<List<Product>>> GetProducts()
        {
            var categories = await Mediator.Send(new GetProductsQuery());

            return Ok(categories);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<Product>> GetProduct([FromRoute] int id)
        {
            var Product = await Mediator.Send(new GetProductQuery(id));

            return Ok(Product);
        }

        [HttpPost()]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<int>> CreateProduct([FromBody] CreateProductCommand command)
        {
            int id = await Mediator.Send(command);

            return Ok(id);
        }

        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<Product>> UpdateProduct([FromRoute] int id, [FromBody] UpdateProductCommand command)
        {
            if (id != command.Id)
            {
                return BadRequest($"Route id {id} does not match request body id {command.Id}");
            }

            await Mediator.Send(command);

            return Ok();
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<Product>> DeleteProduct([FromRoute] int id)
        {
            await Mediator.Send(new DeleteProductCommand(id));

            return Ok();
        }
    }
}