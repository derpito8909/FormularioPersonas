using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Registro.Web.Application.Interfaces;

namespace Registro.Web.Pages.Account;

public class Logout : PageModel
{
    private readonly ITokenStore _store;

    public Logout(ITokenStore store) => _store = store;

    public async Task<IActionResult> OnGetAsync()
    {
        _store.Clear();
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        return RedirectToPage("/Index");
    }
}