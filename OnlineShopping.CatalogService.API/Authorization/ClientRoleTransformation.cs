using System.Security.Claims;
using System.Text.Json;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;
using OnlineShopping.Shared.Infrastructure.Persistence.Options;

namespace OnlineShopping.CatalogService.API.Authorization;

public class ClientRoleTransformation : IClaimsTransformation
{
    private readonly string _clientId;

    public ClientRoleTransformation(IOptions<KeycloakOptions> _keycloakOptions)
    {
        _clientId = _keycloakOptions.Value.ClientId;
    }

    public Task<ClaimsPrincipal> TransformAsync(ClaimsPrincipal principal)
    {
        var result = principal.Clone();
        if (result.Identity is not ClaimsIdentity identity)
            return Task.FromResult(result);

        var resourceAccessValue = principal.FindFirst("resource_access")?.Value;
        if (string.IsNullOrWhiteSpace(resourceAccessValue))
            return Task.FromResult(result);

        var jsonSerializerOptions = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };
        var clients = JsonSerializer.Deserialize<KeycloakJwtClientRoles>(resourceAccessValue, jsonSerializerOptions);
        if (clients == null)
            return Task.FromResult(result);

        var clientRoleContainer = clients.FirstOrDefault(x => x.Key == _clientId);
        if (clientRoleContainer.Key == null)
            return Task.FromResult(result);

        var clientRoles = clientRoleContainer.Value.Roles?
            .Where(role => !string.IsNullOrWhiteSpace(role))
            ?? new List<string>();

        foreach (var role in clientRoles)
            identity.AddClaim(new Claim(ClaimsIdentity.DefaultRoleClaimType, role));

        return Task.FromResult(result);
    }
}