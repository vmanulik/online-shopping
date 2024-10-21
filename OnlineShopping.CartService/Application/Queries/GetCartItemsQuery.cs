using MediatR;
using OnlineShopping.CartService.Domain.Entities;
using OnlineShopping.Shared.Domain.Exceptions;
using OnlineShopping.Shared.Infrastructure;

namespace OnlineShopping.CartService.API.Queries;

public record GetCartItemsQuery(Guid Id) : IRequest<List<Item>>;

public class GetCartItemsQueryHandler : IRequestHandler<GetCartItemsQuery, List<Item>>
{
    private readonly ILiteDbRepository<Cart> _cartRepository;
    private readonly ILiteDbRepository<Item> _itemRepository;

    public GetCartItemsQueryHandler(
            ILiteDbRepository<Cart> cartRepository,
            ILiteDbRepository<Item> itemRepository)
    {
        _cartRepository = cartRepository;
        _itemRepository = itemRepository;
    }

    public async Task<List<Item>> Handle(GetCartItemsQuery request, CancellationToken cancellationToken)
    {
        var carts = _cartRepository.GetAllAsQueryable();
        var cart = await _cartRepository.GetByGuidAsync(request.Id);
        if (cart == null)
        {
            throw new ItemNotFoundException($"Cart ID {request.Id} was not found in the {nameof(Cart)}");
        }

        var items = _itemRepository
            .GetAllAsQueryable()
            .Where(x => x.CartId == request.Id)
            .ToList();

        return items;
    }
}
