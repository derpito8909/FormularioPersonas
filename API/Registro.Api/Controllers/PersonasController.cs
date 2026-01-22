using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Registro.Application.Dtos;
using Registro.Application.Interfaces;

namespace Registro.Api.Controllers;

/// <summary>
/// Endpoints para gestión de personas.
/// </summary>
[ApiController]
[Route("api/personas")]
[Authorize]
public sealed class PersonasController : ControllerBase
{
    private readonly IPersonaService _service;

    public PersonasController(IPersonaService service) => _service = service;
    
    /// <summary>
    /// Lista personas usando el stored procedure <c>dbo.sp_Personas_GetAll</c>.
    /// </summary>
    /// <param name="tipoId">Filtro opcional por tipo de identificación (ej: CC, TI).</param>
    /// <param name="numeroId">Filtro opcional por número de identificación.</param>
    /// <param name="email">Filtro opcional por email.</param>
    /// <param name="ct">Token de cancelación del request.</param>
    /// <returns>Listado de personas ordenadas por fecha de creación descendente.</returns>
    /// <response code="200">Retorna el listado.</response>
    // GET /api/personas?tipoId=CC&numeroId=123&email=a@b.com
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public Task<List<PersonaDto>> GetAll(
        [FromQuery] string? tipoId,
        [FromQuery] string? numeroId,
        [FromQuery] string? email,
        CancellationToken ct)
        => _service.GetAllAsync(tipoId, numeroId, email, ct);

    /// <summary>
    /// Obtiene una persona por id.
    /// </summary>
    /// <param name="id">Identificador de la persona.</param>
    /// <param name="ct">Token de cancelación.</param>
    /// <returns>La persona encontrada.</returns>
    /// <response code="200">Retorna la persona.</response>
    /// <response code="404">No existe una persona con ese id.</response>
    [HttpGet("{id:int}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public Task<PersonaDto> GetById(int id, CancellationToken ct)
        => _service.GetByIdAsync(id, ct);

    /// <summary>
    /// Crea una persona nueva.
    /// </summary>
    /// <param name="request">Datos de la persona.</param>
    /// <param name="ct">Token de cancelación.</param>
    /// <returns>201 con el id creado.</returns>
    /// <response code="201">Persona creada.</response>
    /// <response code="400">Datos inválidos.</response>
    /// <response code="409">Email o documento duplicado (restricción UNIQUE).</response>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Create([FromBody] CreatePersonaRequest request, CancellationToken ct)
    {
        var id = await _service.CreateAsync(request, ct);
        return CreatedAtAction(nameof(GetById), new { id }, new { id });
    }
}