namespace OnlineShopping.Shared.Infrastructure.Persistence.Options;

public class RabbitMqOptions : ApiCredentialsOptions
{
    public string VirtualHost { get; init; }
    public int EventFetchPeriod { get; init; }
}