namespace OnlineShopping.CatalogService.Application;

public interface IAuthenticationTokenExchange
{
    Task<string> GetRefreshTokenAsync(string refreshToken);

    Task<string> GetTokenExchangeAsync(string accessToken);
}