using Microsoft.EntityFrameworkCore;
using Registro.Application.Exceptions;
using Registro.Application.Interfaces;
using Registro.Domain.Entities;
using Registro.Infrastructure.Persistence;
namespace Registro.Infrastructure.repositories;

public class PersonaRepository: IPersonaRepository
{
    private readonly AppDbContext _db;
    public PersonaRepository(AppDbContext db) => _db = db;

    public Task<Persona?> GetByIdAsync(int id, CancellationToken ct)
        => _db.Personas.AsNoTracking().FirstOrDefaultAsync(x => x.PersonaId == id, ct);

    public Task<bool> ExistsByEmailAsync(string email, CancellationToken ct)
        => _db.Personas.AsNoTracking().AnyAsync(x => x.Email == email, ct);

    public Task<bool> ExistsByDocumentoAsync(string tipoId, string numeroId, CancellationToken ct)
        => _db.Personas.AsNoTracking().AnyAsync(x => x.TipoIdentificacion == tipoId && x.NumeroIdentificacion == numeroId, ct);

    public async Task<int> AddAsync(Persona persona, CancellationToken ct)
    {
        _db.Personas.Add(persona);

        try
        {
            await _db.SaveChangesAsync(ct);
            return persona.PersonaId;
        }
        catch (DbUpdateException ex) when (SqlServerErrorCodes.IsUniqueViolation(ex))
        {
            throw new ConflictException(ErrorCodes.DuplicatePersona);
        }
    }

    public Task<List<Persona>> GetAllAsync(string? tipoId, string? numeroId, string? email, CancellationToken ct)
    {
        return _db.Personas
            .FromSqlInterpolated($@"
                EXEC dbo.sp_Personas_GetAll
                    @TipoIdentificacion = {tipoId},
                    @NumeroIdentificacion = {numeroId},
                    @Email = {email}
            ")
            .AsNoTracking()
            .ToListAsync(ct);
    }
}