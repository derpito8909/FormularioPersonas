namespace Registro.Web.Application.Dtos;

public sealed record LoginRequest(string Usuario, string Pass);

public sealed record LoginResponse(string Token);

public sealed record IdResponse(int Id);