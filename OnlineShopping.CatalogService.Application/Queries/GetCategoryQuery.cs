using MediatR;
using OnlineShopping.CartService.Domain.Entities;
using OnlineShopping.Shared.Infrastructure;
using Shared.Application.Exceptions;

namespace OnlineShopping.CartService.API.Queries;

public record GetCategoryQuery(int Id) : IRequest<Category>;

public class GetCategoryQueryHandler : IRequestHandler<GetCategoryQuery, Category>
{
    private readonly ISharedRepository<Category> _categoryRepository;

    public GetCategoryQueryHandler(
            ISharedRepository<Category> categoryRepository)
    {
        _categoryRepository = categoryRepository;
    }

    public async Task<Category> Handle(GetCategoryQuery request, CancellationToken cancellationToken)
    {
        var category = await _categoryRepository.GetByIdAsync(request.Id, cancellationToken);
        if (category == null)
        {
            throw new NotFoundException($"Cart ID {request.Id} was not found in the {nameof(Category)}");
        }

        return category;
    }
}
