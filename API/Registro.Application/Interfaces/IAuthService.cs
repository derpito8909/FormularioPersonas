using Registro.Application.Dtos;
namespace Registro.Application.Interfaces;

public interface IAuthService
{
    Task<string?> LoginAsync(LoginRequest request, CancellationToken ct);
}