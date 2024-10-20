namespace OnlineShopping.Shared.Domain.Entities;

public abstract class BaseEntity
{
    public int Id { get; init; }

    protected BaseEntity() { }

    protected BaseEntity(int id)
    {
        Id = id;
    }
}
