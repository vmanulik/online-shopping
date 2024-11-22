using OnlineShopping.Shared.Domain.Events;
using System.ComponentModel.DataAnnotations.Schema;

namespace OnlineShopping.Shared.Domain.Entities;

public abstract class BaseEntity
{
    private readonly List<BaseEvent> _domainEvents = new List<BaseEvent>();

    public int Id { get; init; }

    [NotMapped]
    public IReadOnlyCollection<BaseEvent> DomainEvents => _domainEvents.AsReadOnly();

    protected BaseEntity() { }

    protected BaseEntity(int id)
    {
        Id = id;
    }

    public void AddDomainEvent(BaseEvent domainEvent)
    {
        _domainEvents.Add(domainEvent);
    }

    public void RemoveDomainEvent(BaseEvent domainEvent)
    {
        _domainEvents.Remove(domainEvent);
    }

    public void ClearDomainEvents()
    {
        _domainEvents.Clear();
    }
}