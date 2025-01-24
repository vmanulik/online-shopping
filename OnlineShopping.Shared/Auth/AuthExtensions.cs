using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using OnlineShopping.Shared.Infrastructure.Persistence.Options;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace OnlineShopping.Shared.Auth;

public static class AuthExtensions
{
    public static void AddKeycloakAuthentication(this IServiceCollection services, KeycloakOptions keycloackOptions)
    {
        services
            .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.Authority = keycloackOptions.Url;
                options.Audience = keycloackOptions.Audience;
                options.RequireHttpsMetadata = false;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateAudience = false,
                    NameClaimType = "preferred_username"
                };
            });
    }

    public static void AddSwaggerKeycloakSecurity(this SwaggerGenOptions options, KeycloakOptions keycloackOptions)
    {
        options.AddSecurityDefinition(
            JwtBearerDefaults.AuthenticationScheme,
            new OpenApiSecurityScheme
            {
                Type = SecuritySchemeType.OpenIdConnect,
                OpenIdConnectUrl = new Uri($"{keycloackOptions.Url}/.well-known/openid-configuration")
            }
        );

        options.OperationFilter<AuthorizationOperationFilter>();
    }
}
