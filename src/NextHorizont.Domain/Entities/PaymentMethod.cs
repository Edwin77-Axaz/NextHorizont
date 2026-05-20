using System;
using System.Collections.Generic;

namespace NextHorizont.Domain.Entities;

public partial class PaymentMethod
{
    protected PaymentMethod()
    {
    }

    public PaymentMethod(Guid id, Guid tenantId, string name, bool requiresReference = false)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("El nombre del método de pago no puede estar vacío.", nameof(name));

        Id = id;
        TenantId = tenantId;
        Name = name;
        RequiresReference = requiresReference;
        CreatedAt = DateTime.UtcNow;
    }

    public Guid Id { get; private set; }

    public Guid TenantId { get; private set; }

    public string Name { get; private set; } = null!;

    public bool RequiresReference { get; private set; }

    public DateTime? CreatedAt { get; private set; }

    // Relaciones
    public virtual ICollection<Payment> Payments { get; private set; } = new List<Payment>();

    public virtual Tenant Tenant { get; private set; } = null!;
}
