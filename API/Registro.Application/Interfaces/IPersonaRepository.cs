using Registro.Domain.Entities;

namespace Registro.Application.Interfaces;

public interface IPersonaRepository
{
    Task<Persona?> GetByIdAsync(int id, CancellationToken ct);
    Task<bool> ExistsByEmailAsync(string email, CancellationToken ct);
    Task<bool> ExistsByDocumentoAsync(string tipoId, string numeroId, CancellationToken ct);
    Task<int> AddAsync(Persona persona, CancellationToken ct);
    Task<List<Persona>> GetAllAsync(string? tipoId, string? numeroId, string? email, CancellationToken ct);
}