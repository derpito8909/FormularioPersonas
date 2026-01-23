using Registro.Web.Application.Dtos;
using Registro.Web.Application.Errors;

namespace Registro.Web.Application.Interfaces;

public interface IAuthClient
{
    Task<ApiCallResult<LoginResponse>> LoginAsync(LoginRequest request, CancellationToken ct);
}