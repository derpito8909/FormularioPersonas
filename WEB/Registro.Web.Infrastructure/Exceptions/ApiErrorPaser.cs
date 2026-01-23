using System.Text.Json;
using Registro.Web.Application.Errors;

namespace Registro.Web.Infrastructure.Exceptions;

public static class ApiErrorParser
{
    private static readonly JsonSerializerOptions JsonOpts = new()
    {
        PropertyNameCaseInsensitive = true
    };

    public static async Task<ApiError> FromHttpResponseAsync(HttpResponseMessage resp, CancellationToken ct)
    {
        var status = (int)resp.StatusCode;
        
        var body = resp.Content is null ? null : await resp.Content.ReadAsStringAsync(ct);
        if (string.IsNullOrWhiteSpace(body))
        {
            return new ApiError
            {
                Type = status == 401 ? "app_error" : "server_error",
                Status = status,
                Code = status == 401 ? "AUTH_REQUIRED" : null,
                Message = DefaultMessageByStatus(status),
                TraceId = null
            };
        }
        
        try
        {
            var validation = JsonSerializer.Deserialize<ValidationErrorEnvelope>(body, JsonOpts);
            if (validation?.Type == "validation_error")
            {
                return new ApiError
                {
                    Type = validation.Type!,
                    Status = validation.Status,
                    TraceId = validation.TraceId,
                    Message = "Revisa los campos del formulario.",
                    Errors = validation.Errors
                };
            }
            
            var appError = JsonSerializer.Deserialize<AppErrorEnvelope>(body, JsonOpts);
            if (!string.IsNullOrWhiteSpace(appError?.Type))
            {
                return new ApiError
                {
                    Type = appError.Type!,
                    Status = appError.Status,
                    TraceId = appError.TraceId,
                    Code = appError.Code,
                    Message = string.IsNullOrWhiteSpace(appError.Message)
                        ? DefaultMessageByStatus(status)
                        : appError.Message
                };
            }
        }
        catch
        {
            //
        }
        
        return new ApiError
        {
            Type = "server_error",
            Status = status,
            Message = DefaultMessageByStatus(status)
        };
    }

    private static string DefaultMessageByStatus(int status) => status switch
    {
        400 => "Solicitud inválida. Revisa los datos.",
        401 => "Debes iniciar sesión para continuar.",
        403 => "No tienes permisos para realizar esta acción.",
        404 => "No se encontró el recurso solicitado.",
        409 => "Conflicto: el registro ya existe o hay datos duplicados.",
        _ => "Ocurrió un error procesando la solicitud."
    };

    private sealed class ValidationErrorEnvelope
    {
        public string? Type { get; set; }
        public int Status { get; set; }
        public string? TraceId { get; set; }
        public Dictionary<string, string[]>? Errors { get; set; }
    }

    private sealed class AppErrorEnvelope
    {
        public string? Type { get; set; }
        public int Status { get; set; }
        public string? TraceId { get; set; }
        public string? Code { get; set; }
        public string? Message { get; set; }
    }
}