using System;

namespace NextHorizont.Application.Interfaces.Contexts;

public interface ITenantProvider
{
    Guid GetTenantId();
}
