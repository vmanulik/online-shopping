using Microsoft.AspNetCore.Mvc;
using OnlineShopping.CartService.API.Commands;
using OnlineShopping.CartService.API.Queries;
using OnlineShopping.CartService.Application.DTOs;
using OnlineShopping.CatalogService.API;

namespace OnlineShopping.CartService.API
{
    [ApiController]
    [Route("[controller]")]
    public class CartController : CartControllerBase
    {
        private readonly ILogger<CartController> _logger;

        public CartController(
            ILogger<CartController> logger)
        {
            _logger = logger;
        }

        [HttpGet("{id}/items")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<List<ItemDTO>>> GetCartItems([FromRoute] Guid id)
        {
            var items = await Mediator.Send(new GetCartItemsQuery(id));

            return Ok(items);
        }

        [HttpPut("{id}/items/add")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> AddItemToCart([FromRoute] Guid id, [FromBody] AddItemToCartCommand command)
        {
            await Mediator.Send(command);

            return NoContent();
        }

        [HttpPut("{id}/items/{itemId}/remove")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> RemoveItemFromCart([FromRoute] Guid id, [FromRoute] int itemId)
        {
            await Mediator.Send(new RemoveItemFromCartCommand(id, itemId));

            return NoContent();
        }
    }
}