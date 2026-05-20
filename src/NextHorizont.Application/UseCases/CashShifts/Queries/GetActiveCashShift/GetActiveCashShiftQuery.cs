using System;
using MediatR;
using NextHorizont.Domain.Entities;

namespace NextHorizont.Application.UseCases.CashShifts.Queries.GetActiveCashShift;

public record GetActiveCashShiftQuery(Guid TenantId, Guid UserId) : IRequest<CashShift?>;
