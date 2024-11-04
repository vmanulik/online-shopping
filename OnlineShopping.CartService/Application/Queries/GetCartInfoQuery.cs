using AutoMapper;
using MediatR;
using OnlineShopping.CartService.Application.DTOs;
using OnlineShopping.CartService.Domain.Entities;
using OnlineShopping.Shared.Infrastructure;
using Shared.Domain.Exceptions;

namespace OnlineShopping.CartService.API.Queries;

public record GetCartInfoQuery(Guid Id) : IRequest<CartDTO>;

public class GetCartInfoQueryHandler : IRequestHandler<GetCartInfoQuery, CartDTO>
{
    private readonly IMapper _mapper;
    private readonly ILiteDbRepository<Cart> _cartRepository;
    private readonly ILiteDbRepository<Item> _itemRepository;

    public GetCartInfoQueryHandler(
        IMapper mapper,
        ILiteDbRepository<Cart> cartRepository,
        ILiteDbRepository<Item> itemRepository)
    {
        _mapper = mapper;
        _cartRepository = cartRepository;
        _itemRepository = itemRepository;
    }

    public async Task<CartDTO> Handle(GetCartInfoQuery request, CancellationToken cancellationToken)
    {
        var carts = _cartRepository.GetAllAsync();
        var cart = await _cartRepository.GetByGuidAsync(request.Id);
        if (cart == null)
        {
            throw new NotFoundException($"Cart ID {request.Id} was not found in the {nameof(Cart)}");
        }

        var cartDTO = _mapper.Map<CartDTO>(cart);
        cartDTO.Quantity = cart.Items.Sum(x => x.Quantity);

        return cartDTO;
    }
}
