using AutoMapper;
using MediatR;
using OnlineShopping.CartService.Domain.Entities;
using OnlineShopping.CatalogService.Application.Categories.DTOs;
using OnlineShopping.Shared.Infrastructure;

namespace OnlineShopping.CatalogService.Application.Products.Queries;

public record GetProductsQuery() : IRequest<List<ProductDTO>>;

public class GetProductsQueryHandler : IRequestHandler<GetProductsQuery, List<ProductDTO>>
{
    private readonly IMapper _mapper;
    private readonly ISharedRepository<Product> _productRepository;

    public GetProductsQueryHandler(
        IMapper mapper,
        ISharedRepository<Product> productRepository)
    {
        _mapper = mapper;
        _productRepository = productRepository;
    }

    public async Task<List<ProductDTO>> Handle(GetProductsQuery request, CancellationToken cancellationToken)
    {
        var products = await _productRepository.GetAllAsync();

        return _mapper.Map<List<ProductDTO>>(products);
    }
}
