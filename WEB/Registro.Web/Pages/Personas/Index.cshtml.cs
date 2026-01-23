using System.Text.Json;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Registro.Web.Application.Dtos;
using Registro.Web.Application.Interfaces;
using Registro.Web.Models;

namespace Registro.Web.Pages.Personas;

[Authorize]
public class Index : PageModel
{
    private readonly IPersonasClient _client;

    public Index(IPersonasClient client) => _client = client;

    public List<PersonaDto> Personas { get; private set; } = new();

    public string? TipoId { get; private set; }
    public string? NumeroId { get; private set; }
    public string? Email { get; private set; }

    public async Task OnGetAsync(string? tipoId, string? numeroId, string? email, CancellationToken ct)
    {
        TipoId = tipoId;
        NumeroId = numeroId;
        Email = email;

        var result = await _client.GetAllAsync(tipoId, numeroId, email, ct);

        if (!result.IsSuccess)
        {
            TempData["UiModal"] = JsonSerializer.Serialize(new UiModal
            {
                Title = "Personas",
                Message = result.Error?.Message ?? "No se pudo cargar la lista.",
                Kind = "error"
            });
            return;
        }

        Personas = result.Data ?? new List<PersonaDto>();
    }
}