using Registro.Application.Dtos;

namespace Registro.Application.Interfaces;

public interface IUsuarioService
{
    Task<int> RegisterAsync(RegisterUsuarioRequest request, CancellationToken ct);
}