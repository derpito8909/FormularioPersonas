using Registro.Web.Application.Dtos;
using Registro.Web.Application.Errors;

namespace Registro.Web.Application.Interfaces;

public interface IPersonasClient
{
    Task<ApiCallResult<IdResponse>> CreateAsync(CreatePersonaRequest request, CancellationToken ct);
    
    Task<ApiCallResult<List<PersonaDto>>> GetAllAsync(
        string? tipoId,
        string? numeroId,
        string? email,
        CancellationToken ct);

    Task<ApiCallResult<PersonaDto>> GetByIdAsync(int id, CancellationToken ct);
}