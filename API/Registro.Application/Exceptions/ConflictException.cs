namespace Registro.Application.Exceptions;

/// <summary>
/// Excepción que indica un conflicto con el estado actual del sistema.
/// Se usa para que el middleware devuelva HTTP 409 (Conflict).
/// </summary>
public sealed class ConflictException : AppException
{
    /// <summary>
    /// Crea una excepción de conflicto con un mensaje específico.
    /// </summary>
    /// <param name="code">code descriptivo del conflicto.</param>
    public ConflictException(string code) : base(code, 409) { }
}