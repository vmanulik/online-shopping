using Microsoft.AspNetCore.Mvc;
using OnlineShopping.CartService.API.Commands;
using OnlineShopping.CartService.API.Queries;
using OnlineShopping.CartService.Domain.Entities;
using OnlineShopping.CatalogService.API;
using OnlineShopping.Shared.Infrastructure;

namespace OnlineShopping.CartService.API
{
    [ApiController]
    [Route("[controller]")]
    public class CartController : CartControllerBase
    {
        private readonly ILogger<CartController> _logger;
        private readonly ILiteDbRepository<Cart> _cartRepository;
        private readonly ILiteDbRepository<Item> _itemRepository;

        public CartController(
            ILogger<CartController> logger,
            ILiteDbRepository<Cart> cartRepository,
            ILiteDbRepository<Item> itemRepository)
        {
            _logger = logger;
            _cartRepository = cartRepository;
            _itemRepository = itemRepository;
        }

        [HttpGet("{id}/items")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<List<Item>>> GetCartItems([FromRoute] Guid id)
        {
            var items = await Mediator.Send(new GetCartItemsQuery(id));

            return Ok(items);
        }

        [HttpPut("{id}/items/add")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> AddItemToCart([FromRoute] Guid id, [FromBody] AddItemToCartCommand command)
        {
            await Mediator.Send(command);

            return Ok();
        }

        [HttpPut("{id}/items/{itemId}/remove")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> RemoveItemFromCart([FromRoute] Guid id, [FromRoute] int itemId)
        {
            await Mediator.Send(new RemoveItemFromCartCommand(id, itemId));

            return Ok();
        }
    }
}