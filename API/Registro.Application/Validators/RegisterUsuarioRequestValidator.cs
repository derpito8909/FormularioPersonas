using FluentValidation;
using Registro.Application.Dtos;

namespace Registro.Application.Validators;

public class RegisterUsuarioRequestValidator: AbstractValidator<RegisterUsuarioRequest>
{
    public RegisterUsuarioRequestValidator()
    {
        RuleFor(x => x.Usuario).NotEmpty().MinimumLength(4).MaximumLength(100);
        RuleFor(x => x.Pass).NotEmpty().MinimumLength(6).MaximumLength(100);
    }
}
