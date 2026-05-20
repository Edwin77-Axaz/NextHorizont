using System;
using System.Collections.Generic;

namespace NextHorizont.Domain.Entities;

public partial class Product
{
    protected Product()
    {
    }

    public Product(Guid id, Guid tenantId, Guid categoryId, Guid? logicalPrinterId, string name, decimal price, string? availability = "TodoElDia")
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("El nombre del producto no puede estar vacío.", nameof(name));
        if (price < 0)
            throw new ArgumentException("El precio no puede ser negativo.", nameof(price));

        Id = id;
        TenantId = tenantId;
        CategoryId = categoryId;
        LogicalPrinterId = logicalPrinterId;
        Name = name;
        Price = price;
        Availability = availability;
        IsActive = true;
        CreatedAt = DateTime.UtcNow;
    }

    public Guid Id { get; private set; }

    public Guid TenantId { get; private set; }

    public Guid CategoryId { get; private set; }

    public Guid? LogicalPrinterId { get; private set; }

    public string Name { get; private set; } = null!;

    public decimal Price { get; private set; }

    public string? Availability { get; private set; }

    public bool? IsActive { get; private set; }

    public DateTime? CreatedAt { get; private set; }

    // Métodos de Negocio
    public void UpdatePrice(decimal newPrice)
    {
        if (newPrice < 0)
            throw new ArgumentException("El precio no puede ser negativo.", nameof(newPrice));
        Price = newPrice;
    }

    public void Activate()
    {
        IsActive = true;
    }

    public void Deactivate()
    {
        IsActive = false;
    }

    public void AssignLogicalPrinter(Guid? printerId)
    {
        LogicalPrinterId = printerId;
    }

    // Relaciones
    public virtual Category Category { get; private set; } = null!;

    public virtual LogicalPrinter? LogicalPrinter { get; private set; }

    public virtual ICollection<OrderLine> OrderLines { get; private set; } = new List<OrderLine>();

    public virtual Tenant Tenant { get; private set; } = null!;

    public virtual ICollection<ModifierGroup> ModifierGroups { get; private set; } = new List<ModifierGroup>();
}
