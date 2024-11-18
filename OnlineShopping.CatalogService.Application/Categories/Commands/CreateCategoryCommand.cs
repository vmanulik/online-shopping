using MediatR;
using OnlineShopping.CartService.Domain.Entities;
using OnlineShopping.Shared.Infrastructure.Abstraction;

namespace OnlineShopping.CatalogService.Application.Categories.Commands;

public record CreateCategoryCommand(
    string Name,
    int? ParentCategoryId,
    string? ImageUrl) : IRequest<int>;

public class CreateCategoryCommandHandler : IRequestHandler<CreateCategoryCommand, int>
{
    private readonly ISharedRepository<Category> _categoryRepository;

    public CreateCategoryCommandHandler(
            ISharedRepository<Category> categoryRepository)
    {
        _categoryRepository = categoryRepository;
    }

    public async Task<int> Handle(CreateCategoryCommand request, CancellationToken cancellationToken)
    {
        var category = new Category(request.Name, request.ParentCategoryId, request.ImageUrl);

        return await _categoryRepository.AddAsync(category, cancellationToken);
    }
}
