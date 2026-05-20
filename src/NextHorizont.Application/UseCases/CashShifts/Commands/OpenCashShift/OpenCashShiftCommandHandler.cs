using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using NextHorizont.Domain.Entities;
using NextHorizont.Domain.Interfaces;

namespace NextHorizont.Application.UseCases.CashShifts.Commands.OpenCashShift;

public class OpenCashShiftCommandHandler : IRequestHandler<OpenCashShiftCommand, Guid>
{
    private readonly ICashShiftRepository _cashShiftRepository;

    public OpenCashShiftCommandHandler(ICashShiftRepository cashShiftRepository)
    {
        _cashShiftRepository = cashShiftRepository;
    }

    public async Task<Guid> Handle(OpenCashShiftCommand request, CancellationToken cancellationToken)
    {
        // Verificar que el usuario no tenga un turno abierto
        var activeShift = await _cashShiftRepository.GetActiveShiftAsync(request.TenantId, request.UserId);
        if (activeShift is not null)
            throw new InvalidOperationException("Este usuario ya tiene un turno de caja abierto. Debe cerrarlo antes de abrir uno nuevo.");

        var shift = new CashShift(
            Guid.NewGuid(),
            request.TenantId,
            request.UserId,
            request.OpeningBalance);

        await _cashShiftRepository.AddAsync(shift);

        return shift.Id;
    }
}
