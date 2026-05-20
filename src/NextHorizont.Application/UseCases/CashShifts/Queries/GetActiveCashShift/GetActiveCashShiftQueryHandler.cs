using System.Threading;
using System.Threading.Tasks;
using MediatR;
using NextHorizont.Domain.Entities;
using NextHorizont.Domain.Interfaces;

namespace NextHorizont.Application.UseCases.CashShifts.Queries.GetActiveCashShift;

public class GetActiveCashShiftQueryHandler : IRequestHandler<GetActiveCashShiftQuery, CashShift?>
{
    private readonly ICashShiftRepository _cashShiftRepository;

    public GetActiveCashShiftQueryHandler(ICashShiftRepository cashShiftRepository)
    {
        _cashShiftRepository = cashShiftRepository;
    }

    public async Task<CashShift?> Handle(GetActiveCashShiftQuery request, CancellationToken cancellationToken)
    {
        return await _cashShiftRepository.GetActiveShiftAsync(request.TenantId, request.UserId);
    }
}
