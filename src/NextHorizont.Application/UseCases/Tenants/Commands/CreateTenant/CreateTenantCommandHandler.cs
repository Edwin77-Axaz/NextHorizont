using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using NextHorizont.Domain.Entities;
using NextHorizont.Domain.Interfaces;

namespace NextHorizont.Application.UseCases.Tenants.Commands.CreateTenant;

public class CreateTenantCommandHandler : IRequestHandler<CreateTenantCommand, Guid>
{
    private readonly ITenantRepository _tenantRepository;

    public CreateTenantCommandHandler(ITenantRepository tenantRepository)
    {
        _tenantRepository = tenantRepository;
    }

    public async Task<Guid> Handle(CreateTenantCommand request, CancellationToken cancellationToken)
    {
        var tenant = new Tenant(
            Guid.NewGuid(), 
            request.Name, 
            request.OrgType);

        await _tenantRepository.AddAsync(tenant);

        return tenant.Id;
    }
}
