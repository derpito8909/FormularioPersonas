namespace Registro.Application.Exceptions;

public static class ErrorCodes
{
    public const string DuplicatePersona = "PERSONA_DUPLICATE";
    public const string DuplicateUsuario = "USUARIO_DUPLICATE";
    public const string InvalidCredentials = "AUTH_INVALID_CREDENTIALS";
    public const string PersonaNotFound = "PERSONA_NOT_FOUND";
}