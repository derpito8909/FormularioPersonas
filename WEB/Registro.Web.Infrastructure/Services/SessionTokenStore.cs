using System.Text;
using Microsoft.AspNetCore.Http;
using Registro.Web.Application.Interfaces;

namespace Registro.Web.Infrastructure.Services;

public sealed class SessionTokenStore : ITokenStore
{
    private const string Key = "jwt_token";
    private readonly IHttpContextAccessor _http;

    public SessionTokenStore(IHttpContextAccessor http) => _http = http;

    public void SaveToken(string token)
    {
        var bytes = Encoding.UTF8.GetBytes(token);
        _http.HttpContext!.Session.Set(Key, bytes);
    }

    public string? GetToken()
    {
        if (_http.HttpContext!.Session.TryGetValue(Key, out var bytes))
            return Encoding.UTF8.GetString(bytes);

        return null;
    }

    public void Clear()
        => _http.HttpContext!.Session.Remove(Key);
}