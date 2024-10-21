using MediatR;
using OnlineShopping.CartService.Domain.Entities;
using OnlineShopping.Shared.Infrastructure;

namespace OnlineShopping.CartService.API.Commands;

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
