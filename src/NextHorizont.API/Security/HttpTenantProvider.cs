using System;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using NextHorizont.Application.Interfaces.Contexts;

namespace NextHorizont.Api.Security;

public class HttpTenantProvider : ITenantProvider
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public HttpTenantProvider(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public Guid GetTenantId()
    {
        // 1. Intentar leer del claim JWT "tenant_id"
        var tenantClaim = _httpContextAccessor.HttpContext?.User?.FindFirst("tenant_id");
        if (tenantClaim is not null && Guid.TryParse(tenantClaim.Value, out var fromJwt))
            return fromJwt;

        // 2. Fallback: leer de la cabecera X-Tenant-Id (útil para endpoints públicos o desarrollo)
        var headerValue = _httpContextAccessor.HttpContext?.Request.Headers["X-Tenant-Id"].FirstOrDefault();
        if (!string.IsNullOrWhiteSpace(headerValue) && Guid.TryParse(headerValue, out var fromHeader))
            return fromHeader;

        throw new UnauthorizedAccessException("No se pudo determinar el TenantId. Asegúrate de enviar un token JWT válido o la cabecera X-Tenant-Id.");
    }
}
