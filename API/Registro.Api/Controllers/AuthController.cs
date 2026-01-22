using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Registro.Application.Interfaces;
using Registro.Application.Dtos;

namespace Registro.Api.Controllers;

/// <summary>
/// Endpoints de autenticación del sistema.
/// </summary>
/// <remarks>
/// Este controlador valida credenciales y emite un JWT firmado para consumir endpoints protegidos.
/// <para>
/// El token contiene claims mínimos: <c>sub</c> (id de usuario) y <c>unique_name</c> (usuario).
/// </para>
/// </remarks>
[ApiController]
[Route("api/auth")]
[AllowAnonymous] 
public sealed class AuthController : ControllerBase
{
    private readonly IAuthService _auth;
    public AuthController(IAuthService auth) => _auth = auth;

    /// <summary>
    /// Valida credenciales y retorna un JWT si son correctas.
    /// </summary>
    /// <param name="request">Credenciales del usuario: usuario y contraseña.</param>
    /// <param name="ct">Token de cancelación del request.</param>
    /// <returns>200 con el JWT si las credenciales son válidas.</returns>
    /// <response code="200">Retorna el token JWT.</response>
    /// <response code="400">Datos inválidos (validación de campos).</response>
    /// <response code="401">Credenciales inválidas.</response>
    [HttpPost("login")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> Login([FromBody] LoginRequest request, CancellationToken ct)
    {
        var token = await _auth.LoginAsync(request, ct);
        return Ok(new { token });
    }
}