using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NextHorizont.Application.UseCases.Tenants.Commands.CreateTenant;
using NextHorizont.Application.UseCases.Tenants.Queries.GetTenantById;

namespace NextHorizont.Api.Controllers;

[Authorize]
public class TenantsController : ApiControllerBase
{
    public record CreateTenantRequest(string Name, string OrgType);

    [HttpPost]
    [AllowAnonymous] // Allow public registration of new tenants
    public async Task<IActionResult> Create([FromBody] CreateTenantRequest request)
    {
        var command = new CreateTenantCommand(request.Name, request.OrgType);
        var result = await Mediator.Send(command);
        return CreatedAtAction(nameof(GetById), new { id = result }, new { Id = result });
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var query = new GetTenantByIdQuery(id);
        var tenant = await Mediator.Send(query);

        if (tenant == null)
            return NotFound();

        return Ok(new
        {
            tenant.Id,
            tenant.Name,
            tenant.OrgType,
            tenant.IsActive,
            tenant.CreatedAt
        });
    }
}
