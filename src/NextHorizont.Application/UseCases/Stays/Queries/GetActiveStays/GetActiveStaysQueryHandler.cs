using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using NextHorizont.Domain.Entities;
using NextHorizont.Domain.Interfaces;

namespace NextHorizont.Application.UseCases.Stays.Queries.GetActiveStays;

public class GetActiveStaysQueryHandler : IRequestHandler<GetActiveStaysQuery, IEnumerable<Stay>>
{
    private readonly IStayRepository _stayRepository;

    public GetActiveStaysQueryHandler(IStayRepository stayRepository)
    {
        _stayRepository = stayRepository;
    }

    public async Task<IEnumerable<Stay>> Handle(GetActiveStaysQuery request, CancellationToken cancellationToken)
    {
        return await _stayRepository.GetActiveStaysAsync(request.TenantId);
    }
}
