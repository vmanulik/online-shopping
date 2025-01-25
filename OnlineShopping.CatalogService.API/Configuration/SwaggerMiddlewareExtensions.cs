using OnlineShopping.Shared.Infrastructure.Persistence.Options;

namespace OnlineShopping.CatalogService.API.Configuration;

public static class SwaggerMiddlewareExtensions
{
    public static IApplicationBuilder UseSwaggerDocumentation(this WebApplication app, IConfiguration configuration)
    {
        var keycloakOptions = configuration.GetSection(nameof(KeycloakOptions)).Get<KeycloakOptions>();

        app.UseSwagger(options =>
            options.RouteTemplate = "api/swagger/{documentName}/swagger.json");
        app.UseSwaggerUI(options =>
        {
            options.SwaggerEndpoint("v1/swagger.json", "Catalog Service API");
            options.RoutePrefix = "api/swagger";
            options.OAuthClientId(keycloakOptions!.ClientId);
            options.OAuthClientSecret(keycloakOptions!.ClientSecret);
            options.EnableTryItOutByDefault();
            //options.EnablePersistAuthorization();
        });

        return app;
    }
}