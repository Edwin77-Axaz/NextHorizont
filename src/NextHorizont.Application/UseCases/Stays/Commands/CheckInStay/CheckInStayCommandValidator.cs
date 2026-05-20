using FluentValidation;

namespace NextHorizont.Application.UseCases.Stays.Commands.CheckInStay;

public class CheckInStayCommandValidator : AbstractValidator<CheckInStayCommand>
{
    public CheckInStayCommandValidator()
    {
        RuleFor(x => x.TenantId)
            .NotEmpty().WithMessage("El TenantId es requerido.");

        RuleFor(x => x.LocationId)
            .NotEmpty().WithMessage("La ubicación (habitación) es requerida.");

        RuleFor(x => x.GuestId)
            .NotEmpty().WithMessage("El huésped es requerido.");

        RuleFor(x => x.CheckOutDate)
            .GreaterThan(x => x.CheckInDate)
            .WithMessage("La fecha de salida debe ser posterior a la fecha de ingreso.");

        RuleFor(x => x.NightlyRate)
            .GreaterThanOrEqualTo(0).WithMessage("La tarifa por noche no puede ser negativa.");
    }
}
