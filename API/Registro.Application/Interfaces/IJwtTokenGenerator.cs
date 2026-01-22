namespace Registro.Application.Interfaces;

/// <summary>
/// Genera JWT firmados para autenticación.
/// </summary>
/// <remarks>
/// Encapsula la creación del token y evita que controladores/servicios construyan JWT manualmente.
/// </remarks>
public interface IJwtTokenGenerator
{
    /// <summary>
    /// Genera un token JWT firmado con claims mínimos del usuario.
    /// </summary>
    /// <param name="usuarioId">Id del usuario (claim sub).</param>
    /// <param name="usuario">Nombre de usuario (claim unique_name).</param>
    /// <returns>JWT serializado en formato string.</returns>
    string GenerateToken(int usuarioId, string usuario);
}