using System;
using MediatR;

namespace NextHorizont.Application.UseCases.CashShifts.Commands.OpenCashShift;

public record OpenCashShiftCommand(
    Guid TenantId,
    Guid UserId,
    decimal OpeningBalance
) : IRequest<Guid>;
