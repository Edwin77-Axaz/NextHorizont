using FluentValidation;

namespace NextHorizont.Application.UseCases.Locations.Commands.ChangeLocationStatus;

public class ChangeLocationStatusCommandValidator : AbstractValidator<ChangeLocationStatusCommand>
{
    public ChangeLocationStatusCommandValidator()
    {
        RuleFor(x => x.TenantId)
            .NotEmpty().WithMessage("El TenantId es requerido.");

        RuleFor(x => x.LocationId)
            .NotEmpty().WithMessage("La ubicación (habitación o mesa) es requerida.");

        RuleFor(x => x.NewStatus)
            .IsInEnum().WithMessage("Estado de habitación inválido.");
    }
}
