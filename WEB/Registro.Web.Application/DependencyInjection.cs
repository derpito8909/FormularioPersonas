using Microsoft.Extensions.DependencyInjection;

namespace Registro.Web.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddWebApplication(this IServiceCollection services)
    {
        return services;
    }
}