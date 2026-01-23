namespace Registro.Web.Application.Errors;

/// <summary>
/// Resultado estándar para llamadas al API desde la Web.
/// Evita exceptions para flujo normal y deja el UI decidir qué mostrar.
/// </summary>
public sealed class ApiCallResult<T>
{
    public bool IsSuccess { get; init; }
    public T? Data { get; init; }
    public ApiError? Error { get; init; }

    public static ApiCallResult<T> Success(T data) => new() { IsSuccess = true, Data = data };
    public static ApiCallResult<T> Fail(ApiError error) => new() { IsSuccess = false, Error = error };
}