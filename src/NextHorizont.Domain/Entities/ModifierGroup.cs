using System;
using System.Collections.Generic;

namespace NextHorizont.Domain.Entities;

public partial class ModifierGroup
{
    protected ModifierGroup()
    {
    }

    public ModifierGroup(Guid id, Guid tenantId, string name, int? minSelections, int? maxSelections)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("El nombre del grupo de modificadores no puede estar vacío.", nameof(name));

        Id = id;
        TenantId = tenantId;
        Name = name;
        MinSelections = minSelections ?? 0;
        MaxSelections = maxSelections ?? 1;
        CreatedAt = DateTime.UtcNow;
    }

    public Guid Id { get; private set; }

    public Guid TenantId { get; private set; }

    public string Name { get; private set; } = null!;

    public int? MinSelections { get; private set; }

    public int? MaxSelections { get; private set; }

    public DateTime? CreatedAt { get; private set; }

    // Relaciones
    public virtual ICollection<Modifier> Modifiers { get; private set; } = new List<Modifier>();

    public virtual Tenant Tenant { get; private set; } = null!;

    public virtual ICollection<Product> Products { get; private set; } = new List<Product>();
}
