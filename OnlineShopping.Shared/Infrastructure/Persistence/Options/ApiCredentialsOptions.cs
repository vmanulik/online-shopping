namespace OnlineShopping.Shared.Infrastructure.Persistence.Options;

public class ApiCredentialsOptions
{
    public string Url { get; init; }
    public int Port { get; init; }
    public string ClientId { get; init; }
    public string ClientSecret { get; init; }
}
