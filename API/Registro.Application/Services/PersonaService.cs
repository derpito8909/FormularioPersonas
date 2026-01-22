using FluentValidation;
using Registro.Application.Dtos;
using Registro.Application.Exceptions;
using Registro.Application.Interfaces;
using Registro.Domain.Entities;

namespace Registro.Application.Services;

public class PersonaService: IPersonaService
{
    private readonly IPersonaRepository _repo;
    private readonly IValidator<CreatePersonaRequest> _validator;

    public PersonaService(IPersonaRepository repo, IValidator<CreatePersonaRequest> validator)
    {
        _repo = repo;
        _validator = validator;
    }

    public async Task<int> CreateAsync(CreatePersonaRequest request, CancellationToken ct)
    {
        await _validator.ValidateAndThrowAsync(request, ct);

        var email = request.Email.Trim().ToLowerInvariant();
        var tipo = request.TipoIdentificacion.Trim().ToUpperInvariant();
        var numero = request.NumeroIdentificacion.Trim();

        if (await _repo.ExistsByEmailAsync(email, ct) ||
            await _repo.ExistsByDocumentoAsync(tipo, numero, ct))
        {
            throw new ConflictException(ErrorCodes.DuplicatePersona);
        }

        var persona = new Persona(
            nombres: request.Nombres,
            apellidos: request.Apellidos,
            tipoId: tipo,
            numeroId: numero,
            email: email);

        return await _repo.AddAsync(persona, ct);
    }

    public async Task<PersonaDto> GetByIdAsync(int id, CancellationToken ct)
    {
        var p = await _repo.GetByIdAsync(id, ct);
        if (p is null) throw new NotFoundException(ErrorCodes.PersonaNotFound);

        return ToDto(p);
    }

    public async Task<List<PersonaDto>> GetAllAsync(string? tipoId, string? numeroId, string? email, CancellationToken ct)
    {
        var list = await _repo.GetAllAsync(tipoId, numeroId, email, ct);
        return list.Select(ToDto).ToList();
    }

    private static PersonaDto ToDto(Persona p) => new(
        p.PersonaId, p.Nombres, p.Apellidos, p.TipoIdentificacion, p.NumeroIdentificacion,
        p.Email, p.IdentificacionCompleta, p.NombreCompleto, p.FechaCreacion
    );
}