using OnlineShopping.CatalogService.Application;
using OnlineShopping.Shared.Infrastructure.Persistence.Options;
using System.Text.Json;

namespace OnlineShopping.CatalogService.API;

public class AuthenticationTokenExchange : IAuthenticationTokenExchange
{
    static readonly HttpClient client = new HttpClient();
    private readonly KeycloakOptions _keycloakOptions;

    public AuthenticationTokenExchange(KeycloakOptions keycloakOptions)
    {
        _keycloakOptions = keycloakOptions;
    }

    public async Task<string> GetRefreshTokenAsync(string refreshToken)
    {
        try
        {
            var payload = new Dictionary<string, string>
            {
                {"grant_type", "refresh_token"},
                {"client_id", _keycloakOptions.ClientId},
                {"client_secret", _keycloakOptions.ClientSecret},
                {"refresh_token", refreshToken }
            };

            HttpResponseMessage tokenResponse = await client.PostAsync(_keycloakOptions.Url, new FormUrlEncodedContent(payload));
            var jsonContent = await tokenResponse.Content.ReadAsStringAsync();

            AuthorizationToken? token = JsonSerializer.Deserialize<AuthorizationToken>(jsonContent);

            return token.AccessToken;
        }
        catch (Exception ex)
        {
            return ex.Message;
        }
    }

    public async Task<string> GetTokenExchangeAsync(string accessToken)
    {
        try
        {
            var form = new Dictionary<string, string>
            {
                {"grant_type", "urn:ietf:params:oauth:grant-type:token-exchange"},
                {"client_id", _keycloakOptions.ClientId},
                {"client_secret", _keycloakOptions.ClientSecret},
                {"audience", "catalog-audience"},
                {"subject_token", accessToken }
            };

            HttpResponseMessage tokenResponse = await client.PostAsync(_keycloakOptions.Url, new FormUrlEncodedContent(form));
            var jsonContent = await tokenResponse.Content.ReadAsStringAsync();

            AuthorizationToken? token = JsonSerializer.Deserialize<AuthorizationToken>(jsonContent);

            return token.AccessToken;
        }
        catch (Exception ex)
        {
            return ex.Message;
        }
    }
}