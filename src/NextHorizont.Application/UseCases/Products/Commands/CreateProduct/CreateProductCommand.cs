using System;
using MediatR;

namespace NextHorizont.Application.UseCases.Products.Commands.CreateProduct;

public record CreateProductCommand(
    Guid TenantId,
    Guid CategoryId,
    Guid? LogicalPrinterId,
    string Name,
    decimal Price,
    string? Availability
) : IRequest<Guid>;
