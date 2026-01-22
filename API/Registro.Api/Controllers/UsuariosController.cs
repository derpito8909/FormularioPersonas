using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Registro.Application.Dtos;
using Registro.Application.Interfaces;

namespace Registro.Api.Controllers;

/// <summary>
/// Endpoints para gestión de usuarios (registro).
/// </summary>
/// <remarks>
/// Este controlador registra usuarios en la tabla <c>dbo.Usuarios</c>.
/// <para>
/// La contraseña no se almacena en texto plano: se guarda como <c>PassHash</c> y <c>PassSalt</c>.
/// </para>
/// <para>
/// El registro NO retorna JWT; el token se obtiene exclusivamente vía <c>POST /api/auth/login</c>.
/// </para>
/// </remarks>
[ApiController]
[Route("api/usuarios")]
[AllowAnonymous]
public sealed class UsuariosController : ControllerBase
{
    private readonly IUsuarioService _service;

    public UsuariosController(IUsuarioService service) => _service = service;

    /// <summary>
    /// Registra un usuario nuevo.
    /// </summary>
    /// <param name="request">Datos de registro: usuario y contraseña.</param>
    /// <param name="ct">Token de cancelación del request.</param>
    /// <returns>201 con el id del usuario creado.</returns>
    /// <response code="201">Usuario creado correctamente.</response>
    /// <response code="400">Datos inválidos (validación de campos).</response>
    /// <response code="409">Usuario duplicado (restricción UNIQUE).</response>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Register([FromBody] RegisterUsuarioRequest request, CancellationToken ct)
    {
        var id = await _service.RegisterAsync(request, ct);
        return Created(new Uri($"/api/usuarios/{id}", UriKind.Relative), new { id });
    }
}