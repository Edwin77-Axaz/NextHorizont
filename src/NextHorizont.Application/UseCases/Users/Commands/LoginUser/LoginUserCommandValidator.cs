using FluentValidation;

namespace NextHorizont.Application.UseCases.Users.Commands.LoginUser;

public class LoginUserCommandValidator : AbstractValidator<LoginUserCommand>
{
    public LoginUserCommandValidator()
    {
        RuleFor(x => x.Username)
            .NotEmpty().WithMessage("El nombre de usuario es requerido.");

        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("La contraseña es requerida.");

        RuleFor(x => x.TenantId)
            .NotEmpty().WithMessage("El TenantId es requerido para el login.");
    }
}
