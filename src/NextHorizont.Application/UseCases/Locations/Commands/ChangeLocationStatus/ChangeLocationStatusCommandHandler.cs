using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using NextHorizont.Domain.Interfaces;

namespace NextHorizont.Application.UseCases.Locations.Commands.ChangeLocationStatus;

public class ChangeLocationStatusCommandHandler : IRequestHandler<ChangeLocationStatusCommand, Guid>
{
    private readonly ILocationRepository _locationRepository;

    public ChangeLocationStatusCommandHandler(ILocationRepository locationRepository)
    {
        _locationRepository = locationRepository;
    }

    public async Task<Guid> Handle(ChangeLocationStatusCommand request, CancellationToken cancellationToken)
    {
        var location = await _locationRepository.GetByIdAsync(request.LocationId, request.TenantId);
        if (location is null)
            throw new InvalidOperationException("Ubicación no encontrada.");

        location.ChangeRoomStatus(request.NewStatus);
        
        await _locationRepository.UpdateAsync(location);

        return location.Id;
    }
}
