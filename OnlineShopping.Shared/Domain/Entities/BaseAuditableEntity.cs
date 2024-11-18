namespace OnlineShopping.Shared.Domain.Entities;

public abstract class BaseAuditableEntity : BaseEntity
{
    public DateTime Created { get; set; }

    public DateTime? LastModified { get; set; }

    public int Version { get; set; }
}
