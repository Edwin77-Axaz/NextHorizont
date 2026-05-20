using System;
using System.Collections.Generic;

namespace NextHorizont.Domain.Entities;

public partial class Area
{
    protected Area()
    {
    }

    public Area(Guid id, Guid tenantId, string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("El nombre del área no puede estar vacío.", nameof(name));

        Id = id;
        TenantId = tenantId;
        Name = name;
        CreatedAt = DateTime.UtcNow;
    }

    public Guid Id { get; private set; }

    public Guid TenantId { get; private set; }

    public string Name { get; private set; } = null!;

    public DateTime? CreatedAt { get; private set; }

    // Relaciones
    public virtual ICollection<Location> Locations { get; private set; } = new List<Location>();

    public virtual Tenant Tenant { get; private set; } = null!;
}
