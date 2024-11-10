using AutoMapper;
using MediatR;
using OnlineShopping.CartService.Domain.Entities;
using OnlineShopping.CatalogService.Application.Categories.DTOs;
using OnlineShopping.CatalogService.Application.Common.Configurations.Sieve;
using OnlineShopping.CatalogService.Application.Common.Models;
using OnlineShopping.Shared.Infrastructure;
using Sieve.Models;

namespace OnlineShopping.CatalogService.Application.Products.Queries;

public record GetProductsQuery(SieveInputModel SieveInput, PaginationModel Pagination) : IRequest<List<ProductDTO>>;

public class GetProductsQueryHandler : IRequestHandler<GetProductsQuery, List<ProductDTO>>
{
    private readonly IMapper _mapper;
    private readonly ProductSieveProcessor _sieveProcessor;
    private readonly ISharedRepository<Product> _productRepository;

    public GetProductsQueryHandler(
        IMapper mapper,
        ProductSieveProcessor sieveProcessor,
        ISharedRepository<Product> productRepository)
    {
        _mapper = mapper;
        _sieveProcessor = sieveProcessor;
        _productRepository = productRepository;
    }

    public async Task<List<ProductDTO>> Handle(GetProductsQuery request, CancellationToken cancellationToken)
    {
        var sieveModel = new SieveModel
        {
            Filters = request.SieveInput.Filter,
            Sorts = request.SieveInput.Sort,
            Page = request.Pagination.PageNumber,
            PageSize = request.Pagination.PageSize,
        };

        var products = _productRepository.GetAllAsQueryable();
        var filteredProducts = _sieveProcessor.Apply(sieveModel, products);

        return _mapper.Map<List<ProductDTO>>(filteredProducts);
    }
}
