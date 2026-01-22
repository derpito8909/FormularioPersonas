using Registro.Application.Dtos;

namespace Registro.Application.Interfaces;

public interface IPersonaService
{
    Task<int> CreateAsync(CreatePersonaRequest request, CancellationToken ct);
    Task<PersonaDto> GetByIdAsync(int id, CancellationToken ct);
    Task<List<PersonaDto>> GetAllAsync(string? tipoId, string? numeroId, string? email, CancellationToken ct);
}