namespace Registro.Application.Exceptions;

public class AppException: Exception
{
    public string Code { get; }
    public int StatusCode { get; }

    public AppException(string code, int statusCode)
    {
        Code = code;
        StatusCode = statusCode;
    }
}