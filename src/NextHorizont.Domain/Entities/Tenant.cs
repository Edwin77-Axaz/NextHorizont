using System;
using System.Collections.Generic;

namespace NextHorizont.Domain.Entities;

public partial class Tenant
{
    // Constructor para EF Core
    protected Tenant()
    {
    }

    // Constructor de Dominio
    public Tenant(Guid id, string name, string orgType)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("El nombre del tenant no puede estar vacío.", nameof(name));
        if (string.IsNullOrWhiteSpace(orgType))
            throw new ArgumentException("El tipo de organización no puede estar vacío.", nameof(orgType));

        Id = id;
        Name = name;
        OrgType = orgType.ToLower();
        IsActive = true;
        CreatedAt = DateTime.UtcNow;
        ActiveModules = "{}"; // JSON vacío por defecto
    }

    public Guid Id { get; private set; }

    public string Name { get; private set; } = null!;

    public string OrgType { get; private set; } = null!;

    public string? ActiveModules { get; private set; }

    public DateTime? CreatedAt { get; private set; }

    public bool? IsActive { get; private set; }

    // Métodos de Negocio
    public void Activate()
    {
        IsActive = true;
    }

    public void Deactivate()
    {
        IsActive = false;
    }

    public void UpdateName(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("El nombre no puede estar vacío.", nameof(name));
        Name = name;
    }

    public void ConfigureModules(string activeModulesJson)
    {
        ActiveModules = activeModulesJson;
    }

    // Relaciones (EF Core las cargará por navegación)
    public virtual ICollection<Area> Areas { get; private set; } = new List<Area>();

    public virtual ICollection<CashShift> CashShifts { get; private set; } = new List<CashShift>();

    public virtual ICollection<Category> Categories { get; private set; } = new List<Category>();

    public virtual ICollection<Guest> Guests { get; private set; } = new List<Guest>();

    public virtual ICollection<Location> Locations { get; private set; } = new List<Location>();

    public virtual ICollection<LogicalPrinter> LogicalPrinters { get; private set; } = new List<LogicalPrinter>();

    public virtual ICollection<ModifierGroup> ModifierGroups { get; private set; } = new List<ModifierGroup>();

    public virtual ICollection<Modifier> Modifiers { get; private set; } = new List<Modifier>();

    public virtual ICollection<OrderLineModifier> OrderLineModifiers { get; private set; } = new List<OrderLineModifier>();

    public virtual ICollection<OrderLine> OrderLines { get; private set; } = new List<OrderLine>();

    public virtual ICollection<Order> Orders { get; private set; } = new List<Order>();

    public virtual ICollection<PaymentMethod> PaymentMethods { get; private set; } = new List<PaymentMethod>();

    public virtual ICollection<Payment> Payments { get; private set; } = new List<Payment>();

    public virtual ICollection<Product> Products { get; private set; } = new List<Product>();

    public virtual ICollection<Role> Roles { get; private set; } = new List<Role>();

    public virtual ICollection<Stay> Stays { get; private set; } = new List<Stay>();

    public virtual ICollection<User> Users { get; private set; } = new List<User>();
}
