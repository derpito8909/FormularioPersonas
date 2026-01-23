namespace Registro.Web.Application.Errors;

/// <summary>
/// Representa un error retornado por el API
/// </summary>
public sealed class ApiError
{
    public string Type { get; init; } = "app_error";
    public int Status { get; init; }
    public string? Code { get; init; }
    public string Message { get; init; } = "Ocurrió un error.";
    public string? TraceId { get; init; }
    
    public Dictionary<string, string[]>? Errors { get; init; }
}