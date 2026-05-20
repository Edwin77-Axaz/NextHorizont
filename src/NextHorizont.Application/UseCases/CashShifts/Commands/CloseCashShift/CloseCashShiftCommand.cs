using System;
using MediatR;

namespace NextHorizont.Application.UseCases.CashShifts.Commands.CloseCashShift;

public record CloseCashShiftCommand(
    Guid TenantId,
    Guid UserId,
    decimal ClosingBalance,
    string? DenominationsClosing
) : IRequest<Guid>;
