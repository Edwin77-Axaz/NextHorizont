using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using NextHorizont.Domain.Entities;
using NextHorizont.Domain.Interfaces;

namespace NextHorizont.Application.UseCases.Guests.Commands.CreateGuest;

public class CreateGuestCommandHandler : IRequestHandler<CreateGuestCommand, Guid>
{
    private readonly IGuestRepository _guestRepository;

    public CreateGuestCommandHandler(IGuestRepository guestRepository)
    {
        _guestRepository = guestRepository;
    }

    public async Task<Guid> Handle(CreateGuestCommand request, CancellationToken cancellationToken)
    {
        // Si tiene documento, verificar que no exista otro huésped con el mismo en el tenant
        if (!string.IsNullOrWhiteSpace(request.IdentificationDocument))
        {
            var existing = await _guestRepository.GetByDocumentAsync(request.IdentificationDocument, request.TenantId);
            if (existing is not null)
                throw new InvalidOperationException($"Ya existe un huésped con el documento '{request.IdentificationDocument}' en este tenant.");
        }

        var guest = new Guest(
            Guid.NewGuid(),
            request.TenantId,
            request.FirstName,
            request.LastName,
            request.IdentificationDocument,
            request.Email,
            request.Phone);

        await _guestRepository.AddAsync(guest);

        return guest.Id;
    }
}
