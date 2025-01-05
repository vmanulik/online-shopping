using Microsoft.AspNetCore.Mvc.ApiExplorer;
using OnlineShopping.Shared.Infrastructure.Persistence.Options;

namespace OnlineShopping.CartService.Configuration;

public static class SwaggerMiddlewareExtensions
{
    public static IApplicationBuilder UseSwaggerDocumentation(this WebApplication app, IConfiguration configuration)
    {
        app.UseSwagger(options =>
            options.RouteTemplate = "api/swagger/{documentName}/swagger.json");

        var apiVersionDescriptionProvider = app.Services
            .GetRequiredService<IApiVersionDescriptionProvider>();

        var keycloakOptions = configuration.GetSection(nameof(KeycloakOptions)).Get<KeycloakOptions>();
        app.UseSwaggerUI(options =>
        {
            foreach (var description in apiVersionDescriptionProvider.ApiVersionDescriptions)
            {
                options.SwaggerEndpoint(
                    $"/api/swagger/{description.GroupName}/swagger.json",
                    description.GroupName.ToUpperInvariant());
                options.RoutePrefix = "api/swagger";
                options.OAuthClientId(keycloakOptions!.ClientId);
                options.OAuthClientSecret(keycloakOptions!.ClientSecret);
            }
        });

        return app;
    }
}