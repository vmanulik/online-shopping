using RabbitMQ.Client.Events;

namespace OnlineShopping.CartService.Infrastructure;

public interface IRabbitMqListener : IDisposable
{
    public event AsyncEventHandler<BasicDeliverEventArgs> ReceivedAsync;

    public Task BasicConsumeAsync(string queue, CancellationToken cancellation);
}
