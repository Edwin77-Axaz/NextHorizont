using System;
using System.Collections.Generic;

namespace NextHorizont.Domain.Entities;

public partial class LogicalPrinter
{
    protected LogicalPrinter()
    {
    }

    public LogicalPrinter(Guid id, Guid tenantId, string name, string? ipAddress = null)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("El nombre de la impresora no puede estar vacío.", nameof(name));

        Id = id;
        TenantId = tenantId;
        Name = name;
        IpAddress = ipAddress;
        CreatedAt = DateTime.UtcNow;
    }

    public Guid Id { get; private set; }

    public Guid TenantId { get; private set; }

    public string Name { get; private set; } = null!;

    public string? IpAddress { get; private set; }

    public DateTime? CreatedAt { get; private set; }

    // Relaciones
    public virtual ICollection<Product> Products { get; private set; } = new List<Product>();

    public virtual Tenant Tenant { get; private set; } = null!;
}
