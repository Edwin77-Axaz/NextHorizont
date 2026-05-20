using FluentValidation;

namespace NextHorizont.Application.UseCases.Orders.Commands.CreateOrder;

public class CreateOrderCommandValidator : AbstractValidator<CreateOrderCommand>
{
    public CreateOrderCommandValidator()
    {
        RuleFor(x => x.TenantId).NotEmpty().WithMessage("TenantId es requerido.");
        RuleFor(x => x.Origin).IsInEnum().WithMessage("Origen de orden inválido.");
    }
}
