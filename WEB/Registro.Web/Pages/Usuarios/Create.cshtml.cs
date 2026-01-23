using System.ComponentModel.DataAnnotations;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Registro.Web.Application.Dtos;
using Registro.Web.Application.Interfaces;
using Registro.Web.Helpers;
using Registro.Web.Models;

namespace Registro.Web.Pages.Usuarios;

public class Create : PageModel
{
    private readonly IUsuariosClient _client;

    public Create(IUsuariosClient client) => _client = client;

    [BindProperty]
    [Required(ErrorMessage = "El usuario es obligatorio.")]
    [StringLength(100, MinimumLength = 4, ErrorMessage = "El usuario debe tener entre 4 y 100 caracteres.")]
    public string Usuario { get; set; } = string.Empty;

    [BindProperty]
    [Required(ErrorMessage = "La contraseña es obligatoria.")]
    [StringLength(100, MinimumLength = 6, ErrorMessage = "La contraseña debe tener entre 6 y 100 caracteres.")]
    public string Pass { get; set; } = string.Empty;

    public async Task<IActionResult> OnPostAsync(CancellationToken ct)
    {
        if (!ModelState.IsValid)
            return Page();

        var result = await _client.RegisterAsync(new RegisterUsuarioRequest(Usuario, Pass), ct);

        if (!result.IsSuccess)
        {
            if (result.Error?.Type == "validation_error")
            {
                ModelState.ApplyApiValidationErrors(result.Error);
                return Page();
            }
            
            TempData["UiModal"] = JsonSerializer.Serialize(new UiModal
            {
                Title = "Crear Usuario",
                Message = result.Error?.Message ?? "No se pudo crear el usuario.",
                Kind = "error"
            });

            return Page();
        }

        TempData["UiModal"] = JsonSerializer.Serialize(new UiModal
        {
            Title = "Crear Usuario",
            Message = $"Usuario creado correctamente. Id: {result.Data!.Id}",
            Kind = "success"
        });
        
        return RedirectToPage("/Usuarios/Create");
    }
}