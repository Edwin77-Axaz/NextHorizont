using System;
using System.Collections.Generic;
using MediatR;
using NextHorizont.Domain.Entities;

namespace NextHorizont.Application.UseCases.Locations.Queries.GetLocationsByArea;

public record GetLocationsByAreaQuery(Guid AreaId, Guid TenantId) : IRequest<IEnumerable<Location>>;
