using System.Reflection;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Registro.Application.Auth;
using Registro.Application.Dtos;
using Registro.Application.Interfaces;
using Registro.Application.Services;

namespace Registro.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

        services.AddScoped<IPersonaService, PersonaService>();
        services.AddScoped<IUsuarioService, UsuarioService>();
        services.AddScoped<IAuthService, AuthService>();

        return services;
    }
}