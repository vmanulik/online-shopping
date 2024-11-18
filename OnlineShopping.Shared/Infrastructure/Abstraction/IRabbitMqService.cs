using OnlineShopping.Shared.Domain.Entities;

namespace OnlineShopping.Shared.Infrastructure.Abstraction
{
    public interface IRabbitMqService
    {
        void SendMessage(IntegrationEvent message);
    }
}
