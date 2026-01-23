using System.Net.Http.Json;
using Registro.Web.Application.Dtos;
using Registro.Web.Application.Errors;
using Registro.Web.Application.Interfaces;

namespace Registro.Web.Infrastructure.Services;

public sealed class UsuariosClient: IUsuariosClient
{
    private readonly HttpClient _http;

    public UsuariosClient(HttpClient http) => _http = http;

    public Task<ApiCallResult<IdResponse>> RegisterAsync(RegisterUsuarioRequest request, CancellationToken ct)
        => ApiHttp.PostAsync<RegisterUsuarioRequest, IdResponse>(_http, "/api/usuarios", request, ct);
    
}