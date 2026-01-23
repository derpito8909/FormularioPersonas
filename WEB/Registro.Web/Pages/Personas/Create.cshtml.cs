using System.ComponentModel.DataAnnotations;
using System.Text.Json;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Registro.Web.Application.Dtos;
using Registro.Web.Application.Interfaces;
using Registro.Web.Helpers;
using Registro.Web.Models;

namespace Registro.Web.Pages.Personas;

[Authorize]
public class Create : PageModel
{
    private readonly IPersonasClient _client;

    public Create(IPersonasClient client) => _client = client;

    [BindProperty]
    [Required(ErrorMessage = "Nombres es obligatorio.")]
    [StringLength(100, ErrorMessage = "Máximo 100 caracteres.")]
    public string Nombres { get; set; } = string.Empty;

    [BindProperty]
    [Required(ErrorMessage = "Apellidos es obligatorio.")]
    [StringLength(100, ErrorMessage = "Máximo 100 caracteres.")]
    public string Apellidos { get; set; } = string.Empty;

    [BindProperty]
    [Required(ErrorMessage = "Tipo de identificación es obligatorio.")]
    public string TipoIdentificacion { get; set; } = string.Empty;

    [BindProperty]
    [Required(ErrorMessage = "Número de identificación es obligatorio.")]
    [StringLength(30, ErrorMessage = "Máximo 30 caracteres.")]
    public string NumeroIdentificacion { get; set; } = string.Empty;

    [BindProperty]
    [Required(ErrorMessage = "Email es obligatorio.")]
    [EmailAddress(ErrorMessage = "Email inválido.")]
    [StringLength(200, ErrorMessage = "Máximo 200 caracteres.")]
    public string Email { get; set; } = string.Empty;

    public async Task<IActionResult> OnPostAsync(CancellationToken ct)
    {
        if (!ModelState.IsValid)
            return Page();

        var request = new CreatePersonaRequest(
            Nombres,
            Apellidos,
            TipoIdentificacion,
            NumeroIdentificacion,
            Email);

        var result = await _client.CreateAsync(request, ct);

        if (!result.IsSuccess)
        {
            if (result.Error?.Status == 401)
                return RedirectToPage("/Account/Login");
            
            if (result.Error?.Type == "validation_error")
            {
                ModelState.ApplyApiValidationErrors(result.Error);
                return Page();
            }
            
            TempData["UiModal"] = JsonSerializer.Serialize(new UiModal
            {
                Title = "Crear Persona",
                Message = result.Error?.Message ?? "No se pudo crear la persona.",
                Kind = "error"
            });

            return Page();
        }

        TempData["UiModal"] = JsonSerializer.Serialize(new UiModal
        {
            Title = "Crear Persona",
            Message = $"Persona creada correctamente. Id: {result.Data!.Id}",
            Kind = "success"
        });

        return RedirectToPage("/Personas/Create");
    }
}