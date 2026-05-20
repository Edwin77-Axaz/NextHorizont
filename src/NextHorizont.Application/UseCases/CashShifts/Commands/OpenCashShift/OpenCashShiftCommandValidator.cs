using FluentValidation;

namespace NextHorizont.Application.UseCases.CashShifts.Commands.OpenCashShift;

public class OpenCashShiftCommandValidator : AbstractValidator<OpenCashShiftCommand>
{
    public OpenCashShiftCommandValidator()
    {
        RuleFor(x => x.TenantId)
            .NotEmpty().WithMessage("El TenantId es requerido.");

        RuleFor(x => x.UserId)
            .NotEmpty().WithMessage("El UserId es requerido.");

        RuleFor(x => x.OpeningBalance)
            .GreaterThanOrEqualTo(0).WithMessage("El saldo de apertura no puede ser negativo.");
    }
}
