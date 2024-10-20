using LiteDB;
using OnlineShopping.Shared.Domain.Entities;

namespace OnlineShopping.CartService.Infrastructure.Persistence.Common;

public abstract class BaseLiteDbEntity : BaseEntity
{
    [BsonId]
    public int Id { get; init; }

    protected BaseLiteDbEntity() { }

    protected BaseLiteDbEntity(int id) : base(id)
    {
    }
}
