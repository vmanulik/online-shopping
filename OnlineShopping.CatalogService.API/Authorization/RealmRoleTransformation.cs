using System.Security.Claims;
using System.Text.Json;
using Microsoft.AspNetCore.Authentication;

namespace OnlineShopping.CatalogService.API.Authorization;

public class RealmRoleTransformation : IClaimsTransformation
{
    public Task<ClaimsPrincipal> TransformAsync(ClaimsPrincipal principal)
    {
        var result = principal.Clone();
        if (result.Identity is not ClaimsIdentity identity)
            return Task.FromResult(result);

        var realmAccess = principal.FindFirst("realm_access")?.Value;
        if (string.IsNullOrWhiteSpace(realmAccess))
            return Task.FromResult(result);

        var jsonSerializerOptions = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };
        var roleContainer = JsonSerializer.Deserialize<KeycloakJwtRoleContainer>(realmAccess, jsonSerializerOptions);
        if (roleContainer == null)
            return Task.FromResult(result);

        var clientRoles = roleContainer.Roles?
            .Where(role => !string.IsNullOrWhiteSpace(role))
            .ToList()
            ?? new List<string>();

        foreach (var role in clientRoles)
            identity.AddClaim(new Claim(ClaimsIdentity.DefaultRoleClaimType, role));

        return Task.FromResult(result);
    }
}