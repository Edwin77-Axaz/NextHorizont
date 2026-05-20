using System;

namespace NextHorizont.Domain.Entities;

public partial class OrderLineModifier
{
    // Constructor para EF Core
    protected OrderLineModifier()
    {
    }

    // Constructor de Dominio
    public OrderLineModifier(Guid id, Guid tenantId, Guid orderLineId, Guid modifierId, decimal? extraPrice)
    {
        Id = id;
        TenantId = tenantId;
        OrderLineId = orderLineId;
        ModifierId = modifierId;
        ExtraPrice = extraPrice ?? 0m;
        CreatedAt = DateTime.UtcNow;
    }

    public Guid Id { get; private set; }

    public Guid TenantId { get; private set; }

    public Guid OrderLineId { get; private set; }

    public Guid ModifierId { get; private set; }

    public decimal? ExtraPrice { get; private set; }

    public DateTime? CreatedAt { get; private set; }

    // Relaciones
    public virtual Modifier Modifier { get; private set; } = null!;

    public virtual OrderLine OrderLine { get; private set; } = null!;

    public virtual Tenant Tenant { get; private set; } = null!;
}
