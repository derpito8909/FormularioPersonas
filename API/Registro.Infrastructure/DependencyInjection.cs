using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Registro.Application.Auth;
using Registro.Application.Dtos;
using Registro.Application.Interfaces;
using Registro.Infrastructure.services;
using Registro.Infrastructure.Persistence;
using Registro.Infrastructure.repositories;

namespace Registro.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddDbContext<AppDbContext>(opt =>
            opt.UseSqlServer(configuration.GetConnectionString("Default")));
        
        services.AddScoped<IPersonaRepository, PersonaRepository>();
        services.AddScoped<IUsuarioRepository, UsuarioRepository>();
        
        services.AddScoped<IPasswordHasher, Pbkdf2PasswordHasher>();
        services.AddScoped<IJwtTokenGenerator, JwtTokenGenerator>();
        
        services.Configure<JwtOptions>(configuration.GetSection("Jwt"));

        return services;
    }
}