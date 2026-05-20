using System;
using System.Collections.Generic;

namespace NextHorizont.Domain.Entities;

public partial class User
{
    protected User()
    {
    }

    public User(Guid id, Guid tenantId, Guid roleId, string username, string passwordHash)
    {
        if (string.IsNullOrWhiteSpace(username))
            throw new ArgumentException("El nombre de usuario no puede estar vacío.", nameof(username));
        if (string.IsNullOrWhiteSpace(passwordHash))
            throw new ArgumentException("El hash de la contraseña no puede estar vacío.", nameof(passwordHash));

        Id = id;
        TenantId = tenantId;
        RoleId = roleId;
        Username = username;
        PasswordHash = passwordHash;
        IsActive = true;
        CreatedAt = DateTime.UtcNow;
    }

    public Guid Id { get; private set; }

    public Guid TenantId { get; private set; }

    public Guid RoleId { get; private set; }

    public string Username { get; private set; } = null!;

    public string PasswordHash { get; private set; } = null!;

    public bool? IsActive { get; private set; }

    public DateTime? CreatedAt { get; private set; }

    // Métodos de Negocio
    public void ChangePassword(string newPasswordHash)
    {
        if (string.IsNullOrWhiteSpace(newPasswordHash))
            throw new ArgumentException("El nuevo hash de la contraseña no puede estar vacío.", nameof(newPasswordHash));
        PasswordHash = newPasswordHash;
    }

    public void AssignRole(Guid newRoleId)
    {
        RoleId = newRoleId;
    }

    public void Activate()
    {
        IsActive = true;
    }

    public void Deactivate()
    {
        IsActive = false;
    }

    // Relaciones
    public virtual ICollection<CashShift> CashShifts { get; private set; } = new List<CashShift>();

    public virtual ICollection<Order> Orders { get; private set; } = new List<Order>();

    public virtual Role Role { get; private set; } = null!;

    public virtual Tenant Tenant { get; private set; } = null!;
}
