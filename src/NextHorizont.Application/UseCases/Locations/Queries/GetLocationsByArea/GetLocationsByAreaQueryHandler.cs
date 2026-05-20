using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using NextHorizont.Domain.Entities;
using NextHorizont.Domain.Interfaces;

namespace NextHorizont.Application.UseCases.Locations.Queries.GetLocationsByArea;

public class GetLocationsByAreaQueryHandler : IRequestHandler<GetLocationsByAreaQuery, IEnumerable<Location>>
{
    private readonly ILocationRepository _locationRepository;

    public GetLocationsByAreaQueryHandler(ILocationRepository locationRepository)
    {
        _locationRepository = locationRepository;
    }

    public async Task<IEnumerable<Location>> Handle(GetLocationsByAreaQuery request, CancellationToken cancellationToken)
    {
        return await _locationRepository.GetByAreaAsync(request.AreaId, request.TenantId);
    }
}
