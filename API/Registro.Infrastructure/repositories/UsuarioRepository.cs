using Microsoft.EntityFrameworkCore;
using Registro.Application.Exceptions;
using Registro.Application.Interfaces;
using Registro.Domain.Entities;
using Registro.Infrastructure.Persistence;

namespace Registro.Infrastructure.repositories;

public sealed class UsuarioRepository: IUsuarioRepository
{
    private readonly AppDbContext _db;
    public UsuarioRepository(AppDbContext db) => _db = db;

    public Task<Usuario?> GetByUsuarioAsync(string usuario, CancellationToken ct)
        => _db.Usuarios.AsNoTracking().FirstOrDefaultAsync(x => x.UsuarioNombre == usuario.Trim(), ct);

    public Task<bool> ExistsByUsuarioAsync(string usuario, CancellationToken ct)
        => _db.Usuarios.AsNoTracking().AnyAsync(x => x.UsuarioNombre == usuario.Trim(), ct);

    public async Task<int> AddAsync(Usuario usuario, CancellationToken ct)
    {
        _db.Usuarios.Add(usuario);

        try
        {
            await _db.SaveChangesAsync(ct);
            return usuario.UsuarioId;
        }
        catch (DbUpdateException ex) when (SqlServerErrorCodes.IsUniqueViolation(ex))
        {
            throw new ConflictException(ErrorCodes.DuplicateUsuario);
        }
    }
}