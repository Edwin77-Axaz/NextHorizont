using FluentValidation;

namespace NextHorizont.Application.UseCases.CashShifts.Commands.CloseCashShift;

public class CloseCashShiftCommandValidator : AbstractValidator<CloseCashShiftCommand>
{
    public CloseCashShiftCommandValidator()
    {
        RuleFor(x => x.TenantId)
            .NotEmpty().WithMessage("El TenantId es requerido.");

        RuleFor(x => x.UserId)
            .NotEmpty().WithMessage("El UserId es requerido.");

        RuleFor(x => x.ClosingBalance)
            .GreaterThanOrEqualTo(0).WithMessage("El saldo de cierre no puede ser negativo.");
    }
}
