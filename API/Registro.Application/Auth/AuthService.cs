using FluentValidation;
using Registro.Application.Exceptions;
using Registro.Application.Interfaces;
using Registro.Application.Dtos;

namespace Registro.Application.Auth;

public sealed class AuthService: IAuthService
{
    private readonly IUsuarioRepository _usuarios;
    private readonly IPasswordHasher _hasher;
    private readonly IJwtTokenGenerator _jwt;
    private readonly IValidator<LoginRequest> _validator;

    public AuthService(
        IUsuarioRepository usuarios,
        IPasswordHasher hasher,
        IJwtTokenGenerator jwt,
        IValidator<LoginRequest> validator)
    {
        _usuarios = usuarios;
        _hasher = hasher;
        _jwt = jwt;
        _validator = validator;
    }

    public async Task<string> LoginAsync(LoginRequest request, CancellationToken ct)
    {
        await _validator.ValidateAndThrowAsync(request, ct);

        var user = request.Usuario.Trim();
        var entity = await _usuarios.GetByUsuarioAsync(user, ct);

        if (entity is null || !_hasher.Verify(request.Pass, entity.PassHash, entity.PassSalt))
            throw new UnauthorizedAppException(ErrorCodes.InvalidCredentials);

        return _jwt.GenerateToken(entity.UsuarioId, entity.UsuarioNombre);
    }
}