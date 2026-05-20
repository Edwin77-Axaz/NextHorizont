using System;
using System.Collections.Generic;
using MediatR;
using NextHorizont.Domain.Entities;

namespace NextHorizont.Application.UseCases.Guests.Queries.SearchGuest;

public record SearchGuestQuery(string SearchTerm, Guid TenantId) : IRequest<IEnumerable<Guest>>;
