using System.Security.Claims;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Logging;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using OnlineShopping.CatalogService.API.BackgroundServices;
using OnlineShopping.CatalogService.Application;
using OnlineShopping.CatalogService.Infrastracture.Interfaces;
using OnlineShopping.CatalogService.Infrastracture.Persistence;
using OnlineShopping.CatalogService.Infrastructure;
using OnlineShopping.CatalogService.Infrastructure.Messaging;
using OnlineShopping.Shared.Infrastructure.Persistence.Options;

namespace OnlineShopping.CatalogService.API
{
    public partial class Program
    {
        private static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            ConfigureServices(builder);

            var app = builder.Build();

            app.UseMigrationsEndPoint();

            using (var scope = app.Services.CreateScope())
            {
                var dbInitializator = scope.ServiceProvider.GetRequiredService<CatalogServiceDbContextInitializer>();

                await dbInitializator.InitialiseAsync();
            }

            var keycloakOptions = app.Configuration.GetSection(nameof(KeycloakOptions)).Get<KeycloakOptions>();
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI(options =>
                {
                    options.SwaggerEndpoint("/swagger/v1/swagger.json", "Catalog Service API");
                    options.OAuthClientId(keycloakOptions!.ClientId);
                    options.OAuthClientSecret(keycloakOptions!.ClientSecret);
                });
            }

            app.MapGet("users/me", (ClaimsPrincipal claimsPrincipal) =>
            {
                claimsPrincipal.Claims.ToDictionary(c => c.Type, c => c.Value);
            }).RequireAuthorization();

            app.UseAuthorization();
            app.UseAuthentication();

            app.UseCors("AllowAllPolicy");
            app.UseHttpsRedirection();
            app.UseResponseCaching();

            app.MapControllers();

            app.Run();
        }

        private static void ConfigureServices(WebApplicationBuilder builder)
        {
            builder.Services.AddControllers(options =>
            {
                options.CacheProfiles.Add("Category",
                    new CacheProfile()
                    {
                        Duration = 600
                    });
                options.CacheProfiles.Add("Product",
                    new CacheProfile()
                    {
                        Duration = 60
                    });
            });

            builder.Services.AddEndpointsApiExplorer();

            builder.Services.AddCatalogInfrastructureServices(builder.Configuration);
            builder.Services.AddCatalogServiceApplicationServices(builder.Configuration);

            builder.Services.Configure<JsonOptions>(options =>
            {
                options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
            });

            builder.Services.AddSingleton<IAuthenticationTokenExchange, AuthenticationTokenExchange>();
            builder.Services.AddSingleton<IRabbitMqService, RabbitMqService>();
            builder.Services.AddHostedService<IntegrationEventSenderService>();

            builder.Services.Configure<RabbitMqOptions>(builder.Configuration.GetSection(nameof(RabbitMqOptions)));
            builder.Services.Configure<KeycloakOptions>(builder.Configuration.GetSection(nameof(KeycloakOptions)));

            var keycloakOptions = builder.Configuration.GetSection(nameof(KeycloakOptions)).Get<KeycloakOptions>();
            AddKeycloakAuthentication(builder.Services, keycloakOptions!);

            builder.Services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "Online Shopping API",
                    Description = "API for managing Products and Categories items",
                    TermsOfService = new Uri("https://example.com/terms"),
                    License = new OpenApiLicense
                    {
                        Name = "MIT License",
                        Url = new Uri("https://opensource.org/license/mit")
                    }
                });

                options.AddSecurityDefinition("Keycloak", new OpenApiSecurityScheme
                {
                    Type = SecuritySchemeType.OAuth2,
                    Flows = new OpenApiOAuthFlows
                    {
                        Implicit = new OpenApiOAuthFlow
                        {
                            AuthorizationUrl = new Uri($"{keycloakOptions!.Url}/protocol/openid-connect/auth"),
                            Scopes = new Dictionary<string, string>
                            {
                                { "openid", "openid" },
                                { "profile", "profile" }
                            }
                        }
                    }
                });

                options.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Id = "Keycloak",
                                Type = ReferenceType.SecurityScheme,
                            },
                            In = ParameterLocation.Header,
                            Name = JwtBearerDefaults.AuthenticationScheme,
                            Scheme = JwtBearerDefaults.AuthenticationScheme,
                        },
                        Array.Empty<string>()
                    }
                });
            });

            //builder.Services.AddCors(corsOptions => corsOptions.AddPolicy("AllowAllPolicy", policyBuilder =>
            //{
            //    policyBuilder.SetIsOriginAllowed(_ => true)
            //        .AllowAnyMethod()
            //        .AllowAnyHeader()
            //        .AllowCredentials();
            //}));
        }

        private static void AddKeycloakAuthentication(IServiceCollection services, KeycloakOptions keycloackOptions)
        {
            IdentityModelEventSource.ShowPII = true;

            //services.AddAuthentication(options =>
            //{
            //    options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            //    options.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            //    options.DefaultChallengeScheme = OpenIdConnectDefaults.AuthenticationScheme;
            //})
            //.AddCookie(cookie =>
            //{
            //    cookie.Cookie.Name = "keycloak.cookie";
            //    cookie.Cookie.MaxAge = TimeSpan.FromMinutes(60);
            //    cookie.Cookie.SecurePolicy = CookieSecurePolicy.SameAsRequest;
            //    cookie.SlidingExpiration = true;
            //})
            //.AddOpenIdConnect(options =>
            //{
            //    options.SignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            //    options.Authority = keycloackOptions.Url;
            //    options.MetadataAddress = $"{keycloackOptions.Url}/.well-known/openid-configuration";
            //    options.ClientId = keycloackOptions.ClientId;
            //    options.ClientSecret = keycloackOptions.ClientSecret;
            //    options.RequireHttpsMetadata = false;
            //    options.GetClaimsFromUserInfoEndpoint = true;
            //    options.Scope.Add("openid");
            //    options.Scope.Add("profile");
            //    options.SaveTokens = true;
            //    options.ResponseType = OpenIdConnectResponseType.Code;
            //    options.NonceCookie.SameSite = SameSiteMode.Unspecified;
            //    options.CorrelationCookie.SameSite = SameSiteMode.Unspecified;
            //    //options.CallbackPath = "/account/logincallback";

            //    //options.Events.OnRedirectToIdentityProvider = async context =>
            //    //{
            //    //    context.ProtocolMessage.IssuerAddress = $"{keycloackOptions.Url}/protocol/openid-connect/auth";
            //    //    await Task.FromResult(0);
            //    //};

            //    options.TokenValidationParameters = new TokenValidationParameters
            //    {
            //        NameClaimType = "name",
            //        RoleClaimType = ClaimTypes.Role,
            //        ValidateIssuer = true,
            //        ValidateLifetime = true
            //    };
            //});

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                    {
                        options.RequireHttpsMetadata = false;
                        options.MetadataAddress = $"{keycloackOptions.Url}/.well-known/openid-configuration";
                        options.Audience = "account";

                        options.TokenValidationParameters = new TokenValidationParameters
                        {
                            ValidIssuer = keycloackOptions.Url,
                            ValidateAudience = false
                        };
                    }
                );

            services.AddAuthorization();
        }
    }
}