using System;
using MediatR;
using NextHorizont.Domain.Entities;

namespace NextHorizont.Application.UseCases.Tenants.Queries.GetTenantById;

public record GetTenantByIdQuery(Guid Id) : IRequest<Tenant?>;
