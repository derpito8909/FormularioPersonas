using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Registro.Web.Application.Interfaces;
using Registro.Web.Infrastructure.Services;

namespace Registro.Web.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddWebInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<ApiOptions.ApiOptions>(configuration.GetSection("Api"));
        var apiBaseUrl = configuration["Api:BaseUrl"] ?? throw new InvalidOperationException("Falta Api:BaseUrl en appsettings.json");

        
        services.AddHttpContextAccessor();
        
        services.AddScoped<ITokenStore, SessionTokenStore>();
        
        services.AddTransient<BearerTokenHandler>();
        
        services.AddHttpClient<IAuthClient, AuthClient>(c =>
        {
            c.BaseAddress = new Uri(apiBaseUrl);
        });

        services.AddHttpClient<IUsuariosClient, UsuariosClient>(c =>
        {
            c.BaseAddress = new Uri(apiBaseUrl);
        });

        services.AddHttpClient<IPersonasClient, PersonasClient>(c =>
            {
                c.BaseAddress = new Uri(apiBaseUrl);
            })
            .AddHttpMessageHandler<BearerTokenHandler>(); 

        return services;
    }
}