using Registro.Domain.Entities;

namespace Registro.Application.Interfaces;

public interface IUsuarioRepository
{
    Task<Usuario?> GetByUsuarioAsync(string usuario, CancellationToken ct);
    Task<bool> ExistsByUsuarioAsync(string usuario, CancellationToken ct);
    Task<int> AddAsync(Usuario usuario, CancellationToken ct);
}