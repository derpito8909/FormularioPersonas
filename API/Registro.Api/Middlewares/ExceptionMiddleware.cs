using System.Net;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Registro.Application.Exceptions;
using Registro.Infrastructure.Persistence;

namespace Registro.Api.Middlewares;
/// <summary>
/// Middleware global de excepciones del API.
/// </summary>
/// <remarks>
/// Este componente es la única fuente responsable de:
/// <list type="bullet">
/// <item><description>Convertir excepciones en códigos HTTP coherentes (400/401/404/409/500).</description></item>
/// <item><description>Definir el formato JSON estándar de error.</description></item>
/// <item><description>Centralizar los mensajes retornados al cliente (evitando textos dispersos en servicios/repositorios/controladores).</description></item>
/// </list>
/// <para>
/// Formato típico de respuesta:
/// <code>
/// {
///   "type": "app_error",
///   "code": "PERSONA_DUPLICATE",
///   "message": "Ya existe una persona con ese email o documento.",
///   "status": 409,
///   "traceId": "..."
/// }
/// </code>
/// </para>
/// <para>
/// Nota: Los detalles internos (stack trace, nombres de tablas, etc.) no se exponen al cliente.
/// </para>
/// </remarks>
public class ExceptionMiddleware: IMiddleware
{
    private static readonly IReadOnlyDictionary<string, string> Messages = new Dictionary<string, string>
    {
        [ErrorCodes.DuplicatePersona] = "Ya existe una persona con ese email o documento.",
        [ErrorCodes.DuplicateUsuario] = "Ya existe ese usuario.",
        [ErrorCodes.InvalidCredentials] = "Credenciales inválidas.",
        [ErrorCodes.PersonaNotFound] = "La persona no existe."
    };
    
    /// <summary>
    /// Ejecuta el siguiente componente del pipeline y captura cualquier excepción no controlada.
    /// </summary>
    /// <param name="context">Contexto HTTP del request actual.</param>
    /// <param name="next">Delegado del siguiente middleware en la cadena.</param>
    /// <returns>Una tarea asíncrona.</returns>
    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        try
        {
            await next(context);
        }
        catch (ValidationException ex)
        {
            context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
            await context.Response.WriteAsJsonAsync(new
            {
                type = "validation_error",
                status = 400,
                traceId = context.TraceIdentifier,
                errors = ex.Errors
                    .GroupBy(e => e.PropertyName)
                    .ToDictionary(g => g.Key, g => g.Select(e => e.ErrorMessage).ToArray())
            });
        }
        catch (AppException ex)
        {
            context.Response.StatusCode = ex.StatusCode;
            await context.Response.WriteAsJsonAsync(new
            {
                type = "app_error",
                code = ex.Code,
                message = Messages.TryGetValue(ex.Code, out var msg) ? msg : "Error de la aplicación.",
                status = ex.StatusCode,
                traceId = context.TraceIdentifier
            });
        }
        catch (DbUpdateException ex) when (SqlServerErrorCodes.IsUniqueViolation(ex))
        {
            context.Response.StatusCode = 409;
            await context.Response.WriteAsJsonAsync(new
            {
                type = "app_error",
                code = ErrorCodes.DuplicatePersona,
                message = Messages[ErrorCodes.DuplicatePersona],
                status = 409,
                traceId = context.TraceIdentifier
            });
        }
        catch (Exception)
        {
            context.Response.StatusCode = 500;
            await context.Response.WriteAsJsonAsync(new
            {
                type = "server_error",
                status = 500,
                traceId = context.TraceIdentifier,
                message = "Error interno del servidor."
            });
        }
    }
}