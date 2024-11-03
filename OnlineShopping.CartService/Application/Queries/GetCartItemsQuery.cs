using AutoMapper;
using MediatR;
using OnlineShopping.CartService.Application.DTOs;
using OnlineShopping.CartService.Domain.Entities;
using OnlineShopping.Shared.Infrastructure;
using Shared.Domain.Exceptions;

namespace OnlineShopping.CartService.API.Queries;

public record GetCartItemsQuery(Guid Id) : IRequest<List<ItemDTO>>;

public class GetCartItemsQueryHandler : IRequestHandler<GetCartItemsQuery, List<ItemDTO>>
{
    private readonly IMapper _mapper;
    private readonly ILiteDbRepository<Cart> _cartRepository;
    private readonly ILiteDbRepository<Item> _itemRepository;

    public GetCartItemsQueryHandler(
        IMapper mapper,
        ILiteDbRepository<Cart> cartRepository,
        ILiteDbRepository<Item> itemRepository)
    {
        _mapper = mapper;
        _cartRepository = cartRepository;
        _itemRepository = itemRepository;
    }

    public async Task<List<ItemDTO>> Handle(GetCartItemsQuery request, CancellationToken cancellationToken)
    {
        var carts = _cartRepository.GetAllAsQueryable();
        var cart = await _cartRepository.GetByGuidAsync(request.Id);
        if (cart == null)
        {
            throw new NotFoundException($"Cart ID {request.Id} was not found in the {nameof(Cart)}");
        }

        var items = _itemRepository
            .GetAllAsQueryable()
            .Where(x => x.CartId == request.Id)
            .ToList();

        return _mapper.Map<List<ItemDTO>>(items);
    }
}
