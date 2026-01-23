using System.Net;
using System.Net.Http.Json;
using System.Text;
using Registro.Web.Application.Dtos;
using Registro.Web.Application.Errors;
using Registro.Web.Application.Interfaces;

namespace Registro.Web.Infrastructure.Services;

public sealed class PersonasClient: IPersonasClient
{
    private readonly HttpClient _http;
    public PersonasClient(HttpClient http) => _http = http;

    public Task<ApiCallResult<IdResponse>> CreateAsync(CreatePersonaRequest request, CancellationToken ct)
        => ApiHttp.PostAsync<CreatePersonaRequest, IdResponse>(_http, "/api/personas", request, ct);

    public Task<ApiCallResult<PersonaDto>> GetByIdAsync(int id, CancellationToken ct)
        => ApiHttp.GetAsync<PersonaDto>(_http, $"/api/personas/{id}", ct);

    public Task<ApiCallResult<List<PersonaDto>>> GetAllAsync(string? tipoId, string? numeroId, string? email, CancellationToken ct)
    {
        var url = BuildQuery("/api/personas", tipoId, numeroId, email);
        return ApiHttp.GetAsync<List<PersonaDto>>(_http, url, ct);
    }

    private static string BuildQuery(string baseUrl, string? tipoId, string? numeroId, string? email)
    {
        var sb = new StringBuilder(baseUrl);
        var has = false;

        void Add(string key, string? value)
        {
            if (string.IsNullOrWhiteSpace(value)) return;
            sb.Append(has ? '&' : '?');
            sb.Append(WebUtility.UrlEncode(key));
            sb.Append('=');
            sb.Append(WebUtility.UrlEncode(value));
            has = true;
        }

        Add("tipoId", tipoId);
        Add("numeroId", numeroId);
        Add("email", email);

        return sb.ToString();
    }
}