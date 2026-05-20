using System;
using System.Collections.Generic;

namespace NextHorizont.Domain.Entities;

public partial class OrderLine
{
    // Constructor para EF Core
    protected OrderLine()
    {
    }

    // Constructor de Dominio
    public OrderLine(Guid id, Guid tenantId, Guid orderId, Guid productId, int quantity, decimal unitPrice)
    {
        if (quantity <= 0)
            throw new ArgumentException("La cantidad debe ser mayor a cero.", nameof(quantity));
        if (unitPrice < 0)
            throw new ArgumentException("El precio unitario no puede ser negativo.", nameof(unitPrice));

        Id = id;
        TenantId = tenantId;
        OrderId = orderId;
        ProductId = productId;
        Quantity = quantity;
        UnitPrice = unitPrice;
        Subtotal = quantity * unitPrice;
        Status = "Pendiente";
        CreatedAt = DateTime.UtcNow;
    }

    public Guid Id { get; private set; }

    public Guid TenantId { get; private set; }

    public Guid OrderId { get; private set; }

    public Guid ProductId { get; private set; }

    public int Quantity { get; private set; }

    public decimal UnitPrice { get; private set; }

    public decimal Subtotal { get; private set; }

    public string Status { get; private set; } = null!;

    public DateTime? CreatedAt { get; private set; }

    // Métodos de Negocio
    public void UpdateQuantity(int quantity)
    {
        if (quantity <= 0)
            throw new ArgumentException("La cantidad debe ser mayor a cero.", nameof(quantity));

        Quantity = quantity;
        Subtotal = Quantity * UnitPrice;
    }

    public void ChangeStatus(string status)
    {
        if (string.IsNullOrWhiteSpace(status))
            throw new ArgumentException("El estado no puede estar vacío.", nameof(status));

        Status = status;
    }

    // Relaciones
    public virtual Order Order { get; private set; } = null!;

    public virtual ICollection<OrderLineModifier> OrderLineModifiers { get; private set; } = new List<OrderLineModifier>();

    public virtual Product Product { get; private set; } = null!;

    public virtual Tenant Tenant { get; private set; } = null!;
}
