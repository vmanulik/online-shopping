using AutoMapper;
using MediatR;
using OnlineShopping.CartService.Domain.Entities;
using OnlineShopping.CatalogService.Application.Categories.DTOs;
using OnlineShopping.CatalogService.Application.Common.Configurations.Sieve;
using OnlineShopping.CatalogService.Application.Common.Models;
using OnlineShopping.Shared.Infrastructure;
using Sieve.Models;

namespace OnlineShopping.CatalogService.Application.Categories.Queries;

public record GetCategoriesQuery(SieveInputModel SieveInput, PaginationModel Pagination) : IRequest<List<CategoryDTO>>;

public class GetCategoriesQueryHandler : IRequestHandler<GetCategoriesQuery, List<CategoryDTO>>
{
    private readonly IMapper _mapper;
    private readonly CategorySieveProcessor _sieveProcessor;
    private readonly ISharedRepository<Category> _categoryRepository;

    public GetCategoriesQueryHandler(
        IMapper mapper,
        CategorySieveProcessor sieveProcessor,
        ISharedRepository<Category> categoryRepository)
    {
        _mapper = mapper;
        _sieveProcessor = sieveProcessor;
        _categoryRepository = categoryRepository;
    }

    public async Task<List<CategoryDTO>> Handle(GetCategoriesQuery request, CancellationToken cancellationToken)
    {
        var sieveModel = new SieveModel
        {
            Filters = request.SieveInput.Filter,
            Sorts = request.SieveInput.Sort,
            Page = request.Pagination.PageNumber,
            PageSize = request.Pagination.PageSize,
        };

        var categories = _categoryRepository.GetAllAsQueryable();
        var filteredCategories = _sieveProcessor.Apply(sieveModel, categories);

        return _mapper.Map<List<CategoryDTO>>(filteredCategories);
    }
}
