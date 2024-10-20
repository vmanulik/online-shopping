using LiteDB;
using Microsoft.AspNetCore.Mvc;
using OnlineShopping.CartService.Domain.Entities;
using OnlineShopping.Shared.Domain.Exceptions;
using OnlineShopping.Shared.Infrastructure;

namespace OnlineShopping.CartService.API
{
    [ApiController]
    [Route("[controller]")]
    public class CartController : ControllerBase
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
        public async Task<List<Item>> GetCartItems([FromRoute] Guid id)
        {
            var cart = await _cartRepository.GetByGuidAsync(id);
            if (cart == null)
            {
                throw new ItemNotFoundException($"Cart ID {id} was not found in the {nameof(Cart)}");
            }

            var items = _itemRepository
                .GetAllAsQueryable()
                .Where(x => x.CartId == id)
                .ToList();

            return items;
        }

        [HttpPut("{id}/items/add")]
        public async Task AddItemToCart([FromRoute] Guid id, [FromBody] Item item)
        {
            Cart cart = await _cartRepository.GetByGuidAsync(id);
            if(cart == null)
            {
                cart = new Cart(id);
                await _cartRepository.AddAsync(cart);

                item.SetCart(cart.Id);
                await _itemRepository.AddAsync(item);
            }
            else
            {
                var items = _itemRepository
                    .GetAllAsQueryable()
                    .Where(x => x.CartId == id);
                cart.Items = items.ToList();

                cart.AddItem(item);

                item = cart.Items.Single(x => x.Id == item.Id);
                await _itemRepository.UpdateAsync(item);
            }
        }

        [HttpPut("{id}/items/remove")]
        public async Task RemoveItemFromCart([FromRoute] Guid id, int itemId)
        {
            Cart cart = await _cartRepository.GetByGuidAsync(id);
            if (cart == null)
            {
                throw new ItemNotFoundException($"Cart ID {id} was not found in the {nameof(Cart)}");
            }
            
            var item = _itemRepository
                .GetAllAsQueryable()
                .Where(x => x.CartId == id);
            if (item == null)
            {
                throw new ItemNotFoundException($"Item ID {itemId} was not found in the {nameof(item)}");
            }


            await _itemRepository.RemoveByIdAsync(itemId);
        }
    }
}