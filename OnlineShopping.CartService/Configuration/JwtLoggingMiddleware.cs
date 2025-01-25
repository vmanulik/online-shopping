using System.IdentityModel.Tokens.Jwt;

namespace OnlineShopping.CartService.Configuration;

public class JwtLoggingMiddleware
{
    private readonly ILogger<JwtLoggingMiddleware> _logger;

    private readonly RequestDelegate _next;

    public JwtLoggingMiddleware(RequestDelegate next, ILogger<JwtLoggingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public Task Invoke(HttpContext context)
    {
        string? authHeader = context.Request.Headers.Authorization;
        string? token = authHeader?.Split(" ")[1];

        if (string.IsNullOrEmpty(token))
        {
            _logger.LogInformation($"{nameof(JwtLoggingMiddleware)}: no token found");
        }
        else
        {
            var jwt = GetJwtToken(token);

            _logger.LogInformation($"{nameof(JwtLoggingMiddleware)}: token found:");
            _logger.LogInformation($"Signature algorithm: {jwt.SignatureAlgorithm}");
            _logger.LogInformation($"Valid To: {jwt.ValidTo}");
            _logger.LogInformation($"Claims:");

            foreach(var claim in jwt.Claims)
            {
                _logger.LogInformation($" {claim.Type}: {claim.Value}");
            }
        }

        return _next(context);
    }

    private static JwtSecurityToken GetJwtToken(string? token)
    {
        var handler = new JwtSecurityTokenHandler();
        var jwt = handler.ReadJwtToken(token);

        return jwt;
    }
}