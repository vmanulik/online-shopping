using AutoMapper;
using MediatR;
using OnlineShopping.CartService.Domain.Entities;
using OnlineShopping.CatalogService.Application.Categories.DTOs;
using OnlineShopping.Shared.Infrastructure.Abstraction;
using Shared.Domain.Exceptions;

namespace OnlineShopping.CatalogService.Application.Categories.Queries;

public record GetCategoryQuery(int Id) : IRequest<CategoryDTO>;

public class GetCategoryQueryHandler : IRequestHandler<GetCategoryQuery, CategoryDTO>
{
    private readonly IMapper _mapper;
    private readonly ISharedRepository<Category> _categoryRepository;

    public GetCategoryQueryHandler(
        IMapper mapper,
        ISharedRepository<Category> categoryRepository)
    {
        _mapper = mapper;
        _categoryRepository = categoryRepository;
    }

    public async Task<CategoryDTO> Handle(GetCategoryQuery request, CancellationToken cancellationToken)
    {
        var category = await _categoryRepository.GetByIdAsync(request.Id, cancellationToken);
        if (category == null)
        {
            throw new NotFoundException($"Cart ID {request.Id} was not found in the {nameof(Category)}");
        }

        return _mapper.Map<CategoryDTO>(category);
    }
}
