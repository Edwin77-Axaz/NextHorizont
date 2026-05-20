using System;
using MediatR;

namespace NextHorizont.Application.UseCases.Tenants.Commands.CreateTenant;

public record CreateTenantCommand(
    string Name,
    string OrgType
) : IRequest<Guid>;
