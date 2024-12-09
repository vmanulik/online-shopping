using MediatR;
using OnlineShopping.CartService.Domain.Events;
using OnlineShopping.Shared.Domain.Entities;
using OnlineShopping.Shared.Domain.Events;
using OnlineShopping.Shared.Infrastructure.Abstraction;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace OnlineShopping.CatalogService.Application.Products.EventHandlers;

public class ProductUpdatedEventHandler : INotificationHandler<ProductUpdatedEvent>
{
    private readonly ISharedRepository<IntegrationEvent> _eventRepository;

    public ProductUpdatedEventHandler(ISharedRepository<IntegrationEvent> eventRepository)
    {
        _eventRepository = eventRepository;
    }

    public async Task Handle(ProductUpdatedEvent notification, CancellationToken cancellationToken)
    {
        var message = new IntegrationEvent()
        {
            Name = Events.ProductUpdate,
            Data = JsonSerializer.Serialize(
                                notification.Product,
                                options: new()
                                {
                                    ReferenceHandler = ReferenceHandler.IgnoreCycles,
                                }
                            )
        };

        _eventRepository.AddWithoutSave(message, cancellationToken);
    }
}
