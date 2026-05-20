using System;
using MediatR;
using NextHorizont.Domain.Enums;

namespace NextHorizont.Application.UseCases.Orders.Commands.CreateOrder;

public record CreateOrderCommand(
    Guid TenantId,
    Guid? LocationId,
    Guid? UserId,
    OrderOrigin Origin
) : IRequest<Guid>;
