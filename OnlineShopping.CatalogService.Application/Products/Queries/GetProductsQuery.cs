using AutoMapper;
using MediatR;
using OnlineShopping.CartService.Domain.Entities;
using OnlineShopping.CatalogService.Application.Categories.DTOs;
using OnlineShopping.CatalogService.Application.Common.Configurations.Sieve;
using OnlineShopping.CatalogService.Application.Common.Interfaces;
using OnlineShopping.CatalogService.Application.Common.Models;
using OnlineShopping.Shared.Infrastructure.Abstraction;
using Sieve.Models;

namespace OnlineShopping.CatalogService.Application.Products.Queries;

public record GetProductsQuery(SieveInputModel SieveInput, PaginationModel Pagination) : IRequest<List<ProductDTO>>;

public class GetProductsQueryHandler : IRequestHandler<GetProductsQuery, List<ProductDTO>>
{
    private readonly IMapper _mapper;
    private readonly ProductSieveProcessor _sieveProcessor;
    private readonly ISharedRepository<Product> _productRepository;
    private readonly ILinksService<ProductDTO> _linksService;

    public GetProductsQueryHandler(
        IMapper mapper,
        ProductSieveProcessor sieveProcessor,
        ISharedRepository<Product> productRepository,
        ILinksService<ProductDTO> linksService)
    {
        _mapper = mapper;
        _sieveProcessor = sieveProcessor;
        _productRepository = productRepository;
        _linksService = linksService;
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

        var dto = _mapper.Map<List<ProductDTO>>(filteredProducts);
        var dtoWithLinks = dto.Select(x => _linksService.CreateLinks(x, nameof(Product)));

        return dtoWithLinks.ToList();
    }
}