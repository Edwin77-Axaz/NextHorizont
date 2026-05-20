using System;
using MediatR;

namespace NextHorizont.Application.UseCases.Stays.Commands.CheckInStay;

public record CheckInStayCommand(
    Guid TenantId,
    Guid LocationId,
    Guid GuestId,
    DateOnly CheckInDate,
    DateOnly CheckOutDate,
    decimal NightlyRate
) : IRequest<Guid>;
