using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using NextHorizont.Domain.Entities;
using NextHorizont.Domain.Interfaces;

namespace NextHorizont.Application.UseCases.Guests.Queries.SearchGuest;

public class SearchGuestQueryHandler : IRequestHandler<SearchGuestQuery, IEnumerable<Guest>>
{
    private readonly IGuestRepository _guestRepository;

    public SearchGuestQueryHandler(IGuestRepository guestRepository)
    {
        _guestRepository = guestRepository;
    }

    public async Task<IEnumerable<Guest>> Handle(SearchGuestQuery request, CancellationToken cancellationToken)
    {
        return await _guestRepository.SearchAsync(request.SearchTerm, request.TenantId);
    }
}
