using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text.Json;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Registro.Web.Application.Dtos;
using Registro.Web.Application.Interfaces;
using Registro.Web.Models;

namespace Registro.Web.Pages.Account;

public class Login : PageModel
{
    private readonly IAuthClient _auth;
    private readonly ITokenStore _tokenStore;

    public Login(IAuthClient auth, ITokenStore tokenStore)
    {
        _auth = auth;
        _tokenStore = tokenStore;
    }

    [BindProperty] public string Usuario { get; set; } = "";
    [BindProperty] public string Pass { get; set; } = "";

    public void OnGet() { }

    public async Task<IActionResult> OnPostAsync(CancellationToken ct)
    {
        var result = await _auth.LoginAsync(new LoginRequest(Usuario, Pass), ct);

        if (!result.IsSuccess)
        {
            TempData["UiModal"] = JsonSerializer.Serialize(new UiModal
            {
                Title = "Login",
                Message = result.Error?.Message ?? "No se pudo iniciar sesión.",
                Kind = "error"
            });

            return Page();
        }
        
        var token = result.Data!.Token;
        _tokenStore.SaveToken(token);
       
        var handler = new JwtSecurityTokenHandler();
        var jwt = handler.ReadJwtToken(token);
        
        var props = new AuthenticationProperties
        {
            IsPersistent = true,
            AllowRefresh = true
        };
        
        var claims = new List<Claim>(jwt.Claims);

        if (!claims.Any(c => c.Type == ClaimTypes.Name))
            claims.Add(new Claim(ClaimTypes.Name, Usuario));
        
        var exp = jwt.Claims.FirstOrDefault(c => c.Type == "exp")?.Value;
        if (long.TryParse(exp, out var expSeconds))
            props.ExpiresUtc = DateTimeOffset.FromUnixTimeSeconds(expSeconds);

        var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
        var principal = new ClaimsPrincipal(identity);

        await HttpContext.SignInAsync(
            CookieAuthenticationDefaults.AuthenticationScheme,
            principal,
            props);

        TempData["UiModal"] = JsonSerializer.Serialize(new UiModal
        {
            Title = "Login",
            Message = "Sesión iniciada correctamente.",
            Kind = "success"
        });

        return RedirectToPage("/Index");
    }
}