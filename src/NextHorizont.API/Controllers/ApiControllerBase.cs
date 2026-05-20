using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using NextHorizont.Application.Interfaces.Contexts;

namespace NextHorizont.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public abstract class ApiControllerBase : ControllerBase
{
    private IMediator? _mediator;
    private ITenantProvider? _tenantProvider;

    protected IMediator Mediator => _mediator ??= HttpContext.RequestServices.GetRequiredService<IMediator>();
    protected Guid TenantId => (_tenantProvider ??= HttpContext.RequestServices.GetRequiredService<ITenantProvider>()).GetTenantId();
}
