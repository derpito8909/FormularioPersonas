using FluentValidation;
using Registro.Application.Dtos;

namespace Registro.Application.Validators;

public class CreatePersonaRequestValidator : AbstractValidator<CreatePersonaRequest>
{
    private static readonly string[] Tipos = ["CC", "TI", "CE", "NIT", "PP"];

    public CreatePersonaRequestValidator()
    {
        RuleFor(x => x.Nombres).NotEmpty().MaximumLength(100);
        RuleFor(x => x.Apellidos).NotEmpty().MaximumLength(100);
        RuleFor(x => x.Email).NotEmpty().EmailAddress().MaximumLength(200);
        RuleFor(x => x.NumeroIdentificacion).NotEmpty().MaximumLength(30);

        RuleFor(x => x.TipoIdentificacion)
            .NotEmpty()
            .Must(t => Tipos.Contains(t.Trim().ToUpperInvariant()));
    }
}