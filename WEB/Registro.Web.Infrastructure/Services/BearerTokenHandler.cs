using System.Net.Http.Headers;
using Registro.Web.Application.Dtos;
using Registro.Web.Application.Interfaces;

namespace Registro.Web.Infrastructure.Services;

public sealed class BearerTokenHandler: DelegatingHandler
{
    private readonly ITokenStore _tokenStore;

    public BearerTokenHandler(ITokenStore tokenStore) => _tokenStore = tokenStore;

    protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        var token = _tokenStore.GetToken();
        if (!string.IsNullOrWhiteSpace(token))
        {
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
        }

        return base.SendAsync(request, cancellationToken);
    }
}