using System.Net.Http.Json;
using Registro.Web.Application.Dtos;
using Registro.Web.Application.Errors;
using Registro.Web.Application.Interfaces;

namespace Registro.Web.Infrastructure.Services;

public sealed class AuthClient: IAuthClient
{
    private readonly HttpClient _http;

    public AuthClient(HttpClient http) => _http = http;

    public async Task<ApiCallResult<LoginResponse>> LoginAsync(LoginRequest request, CancellationToken ct)
        => await ApiHttp.PostAsync<LoginRequest, LoginResponse>(_http, "/api/auth/login", request, ct);
}