using OnlineShopping.Shared.Domain.Entities;

namespace OnlineShopping.CartService.Domain.Entities;

public class Category : BaseEntity
{
    public string Name { get; init; }
    public string? ImageUrl { get; init; }

    public int? ParentCategoryId { get; set; }
    public Category ParentCategory { get; private set; }

    public Category() { }

    public Category(string name, int parentCategoryId)
    {
        Name = name;
        ParentCategoryId = parentCategoryId;
    }
}