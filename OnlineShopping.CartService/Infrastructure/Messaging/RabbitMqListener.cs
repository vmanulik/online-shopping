using Microsoft.Extensions.Options;
using OnlineShopping.Shared.Infrastructure.Persistence.Options;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace OnlineShopping.CartService.Infrastructure.Persistence.Interfaces;

public class RabbitMqListener : IRabbitMqListener, IDisposable
{
    private IConnection _connection;
    private IChannel _channel;

    private AsyncEventingBasicConsumer _consumer;

    private IOptions<RabbitMqOptions> _options;

    public event AsyncEventHandler<BasicDeliverEventArgs> ReceivedAsync
    {
        add => _consumer.ReceivedAsync += value;
        remove => _consumer.ReceivedAsync -= value;
    }

    public RabbitMqListener(IOptions<RabbitMqOptions> options)
    {
        _options = options;

        var factory = new ConnectionFactory()
        {
            HostName = _options.Value.Url,
            VirtualHost = _options.Value.VirtualHost,
            Port = _options.Value.Port,
            UserName = _options.Value.ClientId,
            Password = _options.Value.ClientSecret
        };

        _connection = factory.CreateConnectionAsync().Result;
        _channel = _connection.CreateChannelAsync().Result;
        _consumer = new AsyncEventingBasicConsumer(_channel);
    }

    public async Task BasicConsumeAsync(string queue, CancellationToken cancellation)
    {
        cancellation.ThrowIfCancellationRequested();

        await _channel.BasicConsumeAsync(queue, true, _consumer);
    }

    #region IDisposable

    private bool _disposed;

    protected void Dispose(bool disposing)
    {
        if (!this._disposed)
        {
            if (disposing)
            {
                _channel.Dispose();
                _connection.Dispose();
            }
        }
        this._disposed = true;
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    #endregion
}
