using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using Registro.Web.Application.Errors;
using Registro.Web.Infrastructure.Exceptions;

namespace Registro.Web.Infrastructure.Services;

public static class ApiHttp
{
    private static readonly JsonSerializerOptions JsonOpts = new()
    {
        PropertyNameCaseInsensitive = true
    };

    public static async Task<ApiCallResult<TResponse>> GetAsync<TResponse>(
        HttpClient http,
        string url,
        CancellationToken ct)
    {
        try
        {
            using var resp = await http.GetAsync(url, ct);
            return await HandleResponse<TResponse>(resp, ct);
        }
        catch (TaskCanceledException)
        {
            return ApiCallResult<TResponse>.Fail(new ApiError
            {
                Type = "client_error",
                Status = 0,
                Code = "CLIENT_TIMEOUT",
                Message = "La solicitud tardó demasiado. Intenta de nuevo."
            });
        }
        catch (HttpRequestException)
        {
            return ApiCallResult<TResponse>.Fail(new ApiError
            {
                Type = "client_error",
                Status = 0,
                Code = "CLIENT_NETWORK",
                Message = "No fue posible conectar con el servidor. Verifica que el API esté arriba."
            });
        }
        catch (Exception)
        {
            return ApiCallResult<TResponse>.Fail(new ApiError
            {
                Type = "client_error",
                Status = 0,
                Code = "CLIENT_UNKNOWN",
                Message = "Ocurrió un error inesperado en el cliente."
            });
        }
    }

    public static async Task<ApiCallResult<TResponse>> PostAsync<TRequest, TResponse>(
        HttpClient http,
        string url,
        TRequest request,
        CancellationToken ct)
    {
        try
        {
            using var resp = await http.PostAsJsonAsync(url, request, ct);
            return await HandleResponse<TResponse>(resp, ct);
        }
        catch (TaskCanceledException)
        {
            return ApiCallResult<TResponse>.Fail(new ApiError
            {
                Type = "client_error",
                Status = 0,
                Code = "CLIENT_TIMEOUT",
                Message = "La solicitud tardó demasiado. Intenta de nuevo."
            });
        }
        catch (HttpRequestException)
        {
            return ApiCallResult<TResponse>.Fail(new ApiError
            {
                Type = "client_error",
                Status = 0,
                Code = "CLIENT_NETWORK",
                Message = "No fue posible conectar con el servidor. Verifica que el API esté arriba."
            });
        }
        catch (Exception)
        {
            return ApiCallResult<TResponse>.Fail(new ApiError
            {
                Type = "client_error",
                Status = 0,
                Code = "CLIENT_UNKNOWN",
                Message = "Ocurrió un error inesperado en el cliente."
            });
        }
    }

    private static async Task<ApiCallResult<T>> HandleResponse<T>(HttpResponseMessage resp, CancellationToken ct)
    {
        if (resp.StatusCode == HttpStatusCode.NoContent)
        {
            return ApiCallResult<T>.Success(default!);
        }

        if (resp.IsSuccessStatusCode)
        {
            try
            {
                var data = await resp.Content.ReadFromJsonAsync<T>(JsonOpts, ct);
                
                if (data is null)
                    return ApiCallResult<T>.Success(default!);

                return ApiCallResult<T>.Success(data);
            }
            catch (JsonException)
            {
                return ApiCallResult<T>.Fail(new ApiError
                {
                    Type = "client_error",
                    Status = (int)resp.StatusCode,
                    Code = "CLIENT_BAD_JSON",
                    Message = "El servidor respondió con un formato inesperado."
                });
            }
        }
        
        var apiError = await ApiErrorParser.FromHttpResponseAsync(resp, ct);
        return ApiCallResult<T>.Fail(apiError);
    }
}