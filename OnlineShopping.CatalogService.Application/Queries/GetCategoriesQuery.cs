using MediatR;
using OnlineShopping.CartService.Domain.Entities;
using OnlineShopping.Shared.Infrastructure;

namespace OnlineShopping.CartService.API.Queries;

public record GetCategoriesQuery() : IRequest<List<Category>>;

public class GetCategoriesQueryHandler : IRequestHandler<GetCategoriesQuery, List<Category>>
{
    private readonly ISharedRepository<Category> _categoryRepository;

    public GetCategoriesQueryHandler(
            ISharedRepository<Category> categoryRepository)
    {
        _categoryRepository = categoryRepository;
    }

    public async Task<List<Category>> Handle(GetCategoriesQuery request, CancellationToken cancellationToken)
    {
        return await _categoryRepository.GetAllAsync();
    }
}
