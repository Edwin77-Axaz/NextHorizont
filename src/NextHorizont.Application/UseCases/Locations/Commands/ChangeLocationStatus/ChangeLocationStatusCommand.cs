using System;
using MediatR;
using NextHorizont.Domain.Enums;

namespace NextHorizont.Application.UseCases.Locations.Commands.ChangeLocationStatus;

public record ChangeLocationStatusCommand(
    Guid TenantId,
    Guid LocationId,
    RoomStatus NewStatus
) : IRequest<Guid>;
