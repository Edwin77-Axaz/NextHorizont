using FluentValidation;

namespace NextHorizont.Application.UseCases.Tenants.Commands.CreateTenant;

public class CreateTenantCommandValidator : AbstractValidator<CreateTenantCommand>
{
    public CreateTenantCommandValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("El nombre del tenant es requerido.")
            .MaximumLength(100).WithMessage("El nombre no puede exceder los 100 caracteres.");
            
        RuleFor(x => x.OrgType)
            .NotEmpty().WithMessage("El tipo de organización es requerido.")
            .MaximumLength(50).WithMessage("El tipo de organización no puede exceder los 50 caracteres.");
    }
}
