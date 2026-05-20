using FluentValidation;

namespace NextHorizont.Application.UseCases.Products.Commands.CreateProduct;

public class CreateProductCommandValidator : AbstractValidator<CreateProductCommand>
{
    public CreateProductCommandValidator()
    {
        RuleFor(x => x.TenantId)
            .NotEmpty().WithMessage("El TenantId es requerido.");

        RuleFor(x => x.CategoryId)
            .NotEmpty().WithMessage("La categoría es requerida.");

        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("El nombre del producto es requerido.")
            .MaximumLength(150).WithMessage("El nombre no puede exceder los 150 caracteres.");

        RuleFor(x => x.Price)
            .GreaterThanOrEqualTo(0).WithMessage("El precio no puede ser negativo.");

        RuleFor(x => x.Availability)
            .MaximumLength(50).When(x => !string.IsNullOrWhiteSpace(x.Availability))
            .WithMessage("La disponibilidad no puede exceder los 50 caracteres.");
    }
}
