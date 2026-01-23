using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Registro.Web.Application.Interfaces;

namespace Registro.Web.Middlewares;

public class JwtSessionExpiryMiddleware
{
    private readonly RequestDelegate _next;

    public JwtSessionExpiryMiddleware(RequestDelegate next) => _next = next;

    public async Task InvokeAsync(HttpContext context, ITokenStore tokenStore)
    {
        if (context.User?.Identity?.IsAuthenticated == true)
        {
            var token = tokenStore.GetToken();
            
            if (string.IsNullOrWhiteSpace(token) || IsExpired(token))
            {
                tokenStore.Clear();
                await context.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

                
                if (!context.Request.Path.Equals("/Index", StringComparison.OrdinalIgnoreCase) &&
                    !context.Request.Path.Equals("/", StringComparison.OrdinalIgnoreCase))
                {
                    context.Response.Redirect("/Index");
                    return;
                }
            }
        }

        await _next(context);
    }

    private static bool IsExpired(string token)
    {
        try
        {
            var jwt = new JwtSecurityTokenHandler().ReadJwtToken(token);

            var exp = jwt.Claims.FirstOrDefault(c => c.Type == "exp")?.Value;
            if (!long.TryParse(exp, out var expSeconds))
                return true; 

            var expiresUtc = DateTimeOffset.FromUnixTimeSeconds(expSeconds);
            return expiresUtc <= DateTimeOffset.UtcNow;
        }
        catch
        {
            return true;
        }
    }
}