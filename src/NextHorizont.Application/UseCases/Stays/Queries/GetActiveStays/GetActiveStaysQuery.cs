using System;
using System.Collections.Generic;
using MediatR;
using NextHorizont.Domain.Entities;

namespace NextHorizont.Application.UseCases.Stays.Queries.GetActiveStays;

public record GetActiveStaysQuery(Guid TenantId) : IRequest<IEnumerable<Stay>>;
