namespace OnlineShopping.Shared.Infrastructure.Persistence.Options;

public class KeycloakOptions : ApiCredentialsOptions
{
    public string Audience { get; init; }
}