namespace Registro.Application.Dtos;

public record CreatePersonaRequest(
    string Nombres,
    string Apellidos,
    string TipoIdentificacion,
    string NumeroIdentificacion,
    string Email
);