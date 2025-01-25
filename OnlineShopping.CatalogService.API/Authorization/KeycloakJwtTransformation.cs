using System.Security.Claims;
using System.Text.Json;
using Microsoft.AspNetCore.Authentication;

namespace OnlineShopping.CatalogService.API.Authorization;

public class KeycloakJwtTransformation : IClaimsTransformation
{
    private readonly RealmRoleTransformation _realmRoleTransformation;
    private readonly ClientRoleTransformation _clientRoleTransformation;

    public KeycloakJwtTransformation(RealmRoleTransformation realmRoleTransformation, ClientRoleTransformation clientRoleTransformation)
    {
        _realmRoleTransformation = realmRoleTransformation;
        _clientRoleTransformation = clientRoleTransformation;
    }

    public async Task<ClaimsPrincipal> TransformAsync(ClaimsPrincipal principal)
    {
        var result = principal.Clone();
        result = await _realmRoleTransformation.TransformAsync(result);
        result = await _clientRoleTransformation.TransformAsync(result);
        return result;
    }
}