namespace Registro.Application.Exceptions;

public class UnauthorizedAppException: AppException
{
    public UnauthorizedAppException(string code) : base(code, 401) { }
}