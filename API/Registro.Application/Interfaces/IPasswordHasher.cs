namespace Registro.Application.Interfaces;

/// <summary>
/// Contrato para hashing y verificación de contraseñas.
/// </summary>
/// <remarks>
/// Separa la lógica criptográfica del resto de la aplicación.
/// <para>
/// La implementación recomendada usa un algoritmo de derivación (ej: PBKDF2) y un salt aleatorio por usuario.
/// </para>
/// </remarks>
public interface IPasswordHasher
{
    /// <summary>
    /// Calcula un hash seguro a partir de una contraseña en texto plano y genera un salt aleatorio.
    /// </summary>
    /// <param name="password">Contraseña en texto plano.</param>
    /// <returns>Tupla con el hash y el salt para persistir en base de datos.</returns>
    (byte[] hash, byte[] salt) Hash(string password);
    
    /// <summary>
    /// Verifica si una contraseña coincide con el hash/salt almacenados.
    /// </summary>
    /// <param name="password">Contraseña en texto plano a verificar.</param>
    /// <param name="storedHash">Hash almacenado.</param>
    /// <param name="storedSalt">Salt almacenado.</param>
    /// <returns><c>true</c> si coincide; de lo contrario <c>false</c>.</returns>
    bool Verify(string password, byte[] storedHash, byte[] storedSalt);
}