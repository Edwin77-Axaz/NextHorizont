using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using NextHorizont.Domain.Interfaces;

namespace NextHorizont.Application.UseCases.CashShifts.Commands.CloseCashShift;

public class CloseCashShiftCommandHandler : IRequestHandler<CloseCashShiftCommand, Guid>
{
    private readonly ICashShiftRepository _cashShiftRepository;

    public CloseCashShiftCommandHandler(ICashShiftRepository cashShiftRepository)
    {
        _cashShiftRepository = cashShiftRepository;
    }

    public async Task<Guid> Handle(CloseCashShiftCommand request, CancellationToken cancellationToken)
    {
        var activeShift = await _cashShiftRepository.GetActiveShiftAsync(request.TenantId, request.UserId);
        if (activeShift is null)
            throw new InvalidOperationException("No se encontró un turno de caja abierto para este usuario.");

        activeShift.Close(request.ClosingBalance, request.DenominationsClosing);
        await _cashShiftRepository.UpdateAsync(activeShift);

        return activeShift.Id;
    }
}
