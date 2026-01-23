namespace Registro.Web.Application.Dtos;


public sealed record PersonaDto(
    int PersonaId,
    string Nombres,
    string Apellidos,
    string TipoIdentificacion,
    string NumeroIdentificacion,
    string Email,
    string IdentificacionCompleta,
    string NombreCompleto,
    DateTime FechaCreacion
);