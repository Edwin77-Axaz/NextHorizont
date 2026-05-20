using System;
using MediatR;

namespace NextHorizont.Application.UseCases.Guests.Commands.CreateGuest;

public record CreateGuestCommand(
    Guid TenantId,
    string FirstName,
    string LastName,
    string? IdentificationDocument,
    string? Email,
    string? Phone
) : IRequest<Guid>;
