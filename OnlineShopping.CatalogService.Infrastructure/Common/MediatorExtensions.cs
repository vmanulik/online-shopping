using MediatR;
using OnlineShopping.CatalogService.Infrastracture.Persistence;
using OnlineShopping.Shared.Domain.Entities;

namespace OnlineShopping.CatalogService.Infrastructure.Common;

public static class MediatorExtensions
{
    public static async Task DispatchDomainEvents(this IMediator mediator, CatalogServiceDbContext context)
    {
        var entities = context.ChangeTracker
            .Entries<BaseEntity>()
            .Where(e => e.Entity.DomainEvents.Any())
            .Select(e => e.Entity);

        var domainEvents = entities
            .SelectMany(e => e.DomainEvents)
            .ToList();

        entities.ToList().ForEach(e => e.ClearDomainEvents());

        foreach (var domainEvent in domainEvents)
        {
            await mediator.Publish(domainEvent);
        }
    }
}

