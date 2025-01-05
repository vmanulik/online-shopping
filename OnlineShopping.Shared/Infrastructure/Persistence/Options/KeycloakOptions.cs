namespace OnlineShopping.Shared.Infrastructure.Persistence.Options;

public class KeycloakOptions : ApiCredentialsOptions
{
    public string LoginCallback { get; init; }
}