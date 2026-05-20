using FluentValidation;

namespace NextHorizont.Application.UseCases.Guests.Commands.CreateGuest;

public class CreateGuestCommandValidator : AbstractValidator<CreateGuestCommand>
{
    public CreateGuestCommandValidator()
    {
        RuleFor(x => x.TenantId)
            .NotEmpty().WithMessage("El TenantId es requerido.");

        RuleFor(x => x.FirstName)
            .NotEmpty().WithMessage("El nombre del huésped es requerido.")
            .MaximumLength(100).WithMessage("El nombre no puede exceder los 100 caracteres.");

        RuleFor(x => x.LastName)
            .NotEmpty().WithMessage("El apellido del huésped es requerido.")
            .MaximumLength(100).WithMessage("El apellido no puede exceder los 100 caracteres.");

        RuleFor(x => x.Email)
            .EmailAddress().When(x => !string.IsNullOrWhiteSpace(x.Email))
            .WithMessage("El correo electrónico no es válido.");

        RuleFor(x => x.Phone)
            .MaximumLength(20).When(x => !string.IsNullOrWhiteSpace(x.Phone))
            .WithMessage("El teléfono no puede exceder los 20 caracteres.");

        RuleFor(x => x.IdentificationDocument)
            .MaximumLength(30).When(x => !string.IsNullOrWhiteSpace(x.IdentificationDocument))
            .WithMessage("El documento de identificación no puede exceder los 30 caracteres.");
    }
}
