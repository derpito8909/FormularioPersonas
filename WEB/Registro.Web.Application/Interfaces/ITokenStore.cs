namespace Registro.Web.Application.Interfaces;

public interface ITokenStore
{
    void SaveToken(string token);
    string? GetToken();
    void Clear();
}