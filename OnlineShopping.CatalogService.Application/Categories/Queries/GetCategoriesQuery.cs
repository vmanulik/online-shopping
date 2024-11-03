using AutoMapper;
using MediatR;
using OnlineShopping.CartService.Domain.Entities;
using OnlineShopping.CatalogService.Application.Categories.DTOs;
using OnlineShopping.Shared.Infrastructure;

namespace OnlineShopping.CatalogService.Application.Categories.Queries;

public record GetCategoriesQuery() : IRequest<List<CategoryDTO>>;

public class GetCategoriesQueryHandler : IRequestHandler<GetCategoriesQuery, List<CategoryDTO>>
{
    private readonly IMapper _mapper;
    private readonly ISharedRepository<Category> _categoryRepository;

    public GetCategoriesQueryHandler(
        IMapper mapper,
        ISharedRepository<Category> categoryRepository)
    {
        _mapper = mapper;
        _categoryRepository = categoryRepository;
    }

    public async Task<List<CategoryDTO>> Handle(GetCategoriesQuery request, CancellationToken cancellationToken)
    {
        var categories = await _categoryRepository.GetAllAsync();

        return _mapper.Map<List<CategoryDTO>>(categories);
    }
}
