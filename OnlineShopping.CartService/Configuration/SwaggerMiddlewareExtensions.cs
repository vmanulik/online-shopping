using Microsoft.AspNetCore.Mvc.ApiExplorer;

namespace OnlineShopping.CartService.Configuration;

public static class SwaggerMiddlewareExtensions
{
    public static IApplicationBuilder UseSwaggerDocumentation(this WebApplication app, IConfiguration configuration)
    {
        app.UseSwagger(options =>
            options.RouteTemplate = "api/swagger/{documentName}/swagger.json");

        var apiVersionDescriptionProvider = app.Services
            .GetRequiredService<IApiVersionDescriptionProvider>();

        app.UseSwaggerUI(options =>
        {
            foreach (var description in apiVersionDescriptionProvider.ApiVersionDescriptions)
            {
                options.SwaggerEndpoint(
                    $"/api/swagger/{description.GroupName}/swagger.json",
                    description.GroupName.ToUpperInvariant());
                options.RoutePrefix = "api/swagger";
            }
        });

        return app;
    }
}