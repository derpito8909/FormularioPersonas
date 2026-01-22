using FluentValidation;
using Registro.Application.Dtos;
using Registro.Application.Exceptions;
using Registro.Application.Interfaces;
using Registro.Domain.Entities;

namespace Registro.Application.Services;

public class UsuarioService: IUsuarioService
{
    private readonly IUsuarioRepository _repo;
    private readonly IPasswordHasher _hasher;
    private readonly IValidator<RegisterUsuarioRequest> _validator;

    public UsuarioService(IUsuarioRepository repo, IPasswordHasher hasher, IValidator<RegisterUsuarioRequest> validator)
    {
        _repo = repo;
        _hasher = hasher;
        _validator = validator;
    }

    public async Task<int> RegisterAsync(RegisterUsuarioRequest request, CancellationToken ct)
    {
        await _validator.ValidateAndThrowAsync(request, ct);

        var user = request.Usuario.Trim();

        if (await _repo.ExistsByUsuarioAsync(user, ct))
            throw new ConflictException(ErrorCodes.DuplicateUsuario);

        var (hash, salt) = _hasher.Hash(request.Pass);
        var entity = new Usuario(user, hash, salt);

        return await _repo.AddAsync(entity, ct);
    }
}