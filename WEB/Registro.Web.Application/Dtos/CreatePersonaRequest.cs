namespace Registro.Web.Application.Dtos;

public sealed record CreatePersonaRequest(
    string Nombres,
    string Apellidos,
    string TipoIdentificacion,
    string NumeroIdentificacion,
    string Email
);