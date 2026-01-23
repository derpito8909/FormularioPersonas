using Registro.Web.Application.Dtos;
using Registro.Web.Application.Errors;

namespace Registro.Web.Application.Interfaces;

public interface IUsuariosClient
{
    Task<ApiCallResult<IdResponse>> RegisterAsync(
        RegisterUsuarioRequest request,
        CancellationToken ct);
}