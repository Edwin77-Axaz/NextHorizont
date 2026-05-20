using System;

namespace NextHorizont.Domain.Entities;

public partial class Payment
{
    // Constructor para EF Core
    protected Payment()
    {
    }

    // Constructor de Dominio
    public Payment(Guid id, Guid tenantId, Guid orderId, Guid paymentMethodId, Guid cashShiftId, decimal amount, string? referenceCode = null)
    {
        if (amount <= 0)
            throw new ArgumentException("El monto del pago debe ser mayor a cero.", nameof(amount));

        Id = id;
        TenantId = tenantId;
        OrderId = orderId;
        PaymentMethodId = paymentMethodId;
        CashShiftId = cashShiftId;
        Amount = amount;
        ReferenceCode = referenceCode;
        CreatedAt = DateTime.UtcNow;
    }

    public Guid Id { get; private set; }

    public Guid TenantId { get; private set; }

    public Guid OrderId { get; private set; }

    public Guid CashShiftId { get; private set; }

    public Guid PaymentMethodId { get; private set; }

    public string? ReferenceCode { get; private set; }

    public decimal Amount { get; private set; }

    public DateTime? CreatedAt { get; private set; }

    // Relaciones
    public virtual CashShift CashShift { get; private set; } = null!;

    public virtual Order Order { get; private set; } = null!;

    public virtual PaymentMethod PaymentMethod { get; private set; } = null!;

    public virtual Tenant Tenant { get; private set; } = null!;
}
