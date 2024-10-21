using OnlineShopping.Shared.Domain.Entities;

namespace OnlineShopping.CartService.Domain.Entities;

public class Category : BaseEntity
{
    public string Name { get; private set; }
    public string? ImageUrl { get; private set; }

    public int? ParentCategoryId { get; private set; }
    public Category ParentCategory { get; private set; }

    public Category() { }

    public Category(string name, int? parentCategoryId, string? imageUrl = null)
    {
        Name = name;
        ParentCategoryId = parentCategoryId;
        ImageUrl = imageUrl;
    }

    public void Update(
        string name,
        string? imageUrl,
        int? parentCategoryId)
    {
        Name = name;
        ImageUrl = imageUrl;
        ParentCategoryId = parentCategoryId;
    }
}