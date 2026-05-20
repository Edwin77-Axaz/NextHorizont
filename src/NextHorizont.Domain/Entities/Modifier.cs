using System;
using System.Collections.Generic;

namespace NextHorizont.Domain.Entities;

public partial class Modifier
{
    protected Modifier()
    {
    }

    public Modifier(Guid id, Guid tenantId, Guid modifierGroupId, string name, decimal? extraPrice)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("El nombre del modificador no puede estar vacío.", nameof(name));
        if (extraPrice < 0)
            throw new ArgumentException("El precio extra no puede ser negativo.", nameof(extraPrice));

        Id = id;
        TenantId = tenantId;
        ModifierGroupId = modifierGroupId;
        Name = name;
        ExtraPrice = extraPrice ?? 0m;
        CreatedAt = DateTime.UtcNow;
    }

    public Guid Id { get; private set; }

    public Guid TenantId { get; private set; }

    public Guid ModifierGroupId { get; private set; }

    public string Name { get; private set; } = null!;

    public decimal? ExtraPrice { get; private set; }

    public DateTime? CreatedAt { get; private set; }

    // Relaciones
    public virtual ModifierGroup ModifierGroup { get; private set; } = null!;

    public virtual ICollection<OrderLineModifier> OrderLineModifiers { get; private set; } = new List<OrderLineModifier>();

    public virtual Tenant Tenant { get; private set; } = null!;
}
