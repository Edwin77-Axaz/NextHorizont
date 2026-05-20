using System;
using System.Collections.Generic;

namespace NextHorizont.Domain.Entities;

public partial class Role
{
    protected Role()
    {
    }

    public Role(Guid id, Guid tenantId, string name, string? permissionsJson = "[]")
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("El nombre del rol no puede estar vacío.", nameof(name));

        Id = id;
        TenantId = tenantId;
        Name = name;
        PermissionsJson = permissionsJson;
        CreatedAt = DateTime.UtcNow;
    }

    public Guid Id { get; private set; }

    public Guid TenantId { get; private set; }

    public string Name { get; private set; } = null!;

    public string? PermissionsJson { get; private set; }

    public DateTime? CreatedAt { get; private set; }

    // Métodos de Negocio
    public void UpdateName(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("El nombre del rol no puede estar vacío.", nameof(name));
        Name = name;
    }

    public void UpdatePermissions(string permissionsJson)
    {
        PermissionsJson = permissionsJson;
    }

    // Relaciones
    public virtual Tenant Tenant { get; private set; } = null!;

    public virtual ICollection<User> Users { get; private set; } = new List<User>();
}
