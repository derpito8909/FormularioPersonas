using System.Text.Json;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Registro.Web.Application.Dtos;
using Registro.Web.Application.Interfaces;
using Registro.Web.Models;

namespace Registro.Web.Pages.Personas;

[Authorize]
public class Details : PageModel
{
    private readonly IPersonasClient _client;

    public Details(IPersonasClient client) => _client = client;

    public PersonaDto? Persona { get; private set; }

    public async Task<IActionResult> OnGetAsync(int id, CancellationToken ct)
    {
        var result = await _client.GetByIdAsync(id, ct);

        if (!result.IsSuccess)
        {
            TempData["UiModal"] = JsonSerializer.Serialize(new UiModal
            {
                Title = "Detalle Persona",
                Message = result.Error?.Message ?? "No se pudo cargar el detalle.",
                Kind = "error"
            });
            
            return RedirectToPage("/Personas/Index");
        }

        Persona = result.Data;
        return Page();
    }
}