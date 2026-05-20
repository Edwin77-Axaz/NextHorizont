using System;
using System.Collections.Generic;

namespace NextHorizont.Domain.Entities;

public partial class Guest
{
    protected Guest()
    {
    }

    public Guest(Guid id, Guid tenantId, string firstName, string lastName, string? identificationDocument = null, string? email = null, string? phone = null)
    {
        if (string.IsNullOrWhiteSpace(firstName))
            throw new ArgumentException("El nombre del huésped no puede estar vacío.", nameof(firstName));
        if (string.IsNullOrWhiteSpace(lastName))
            throw new ArgumentException("El apellido del huésped no puede estar vacío.", nameof(lastName));

        Id = id;
        TenantId = tenantId;
        FirstName = firstName;
        LastName = lastName;
        IdentificationDocument = identificationDocument;
        Email = email;
        Phone = phone;
        CreatedAt = DateTime.UtcNow;
    }

    public Guid Id { get; private set; }

    public Guid TenantId { get; private set; }

    public string FirstName { get; private set; } = null!;

    public string LastName { get; private set; } = null!;

    public string? IdentificationDocument { get; private set; }

    public string? Email { get; private set; }

    public string? Phone { get; private set; }

    public DateTime? CreatedAt { get; private set; }

    // Métodos de Negocio
    public void UpdateProfile(string firstName, string lastName, string? email, string? phone, string? doc)
    {
        if (string.IsNullOrWhiteSpace(firstName))
            throw new ArgumentException("El nombre no puede estar vacío.", nameof(firstName));
        if (string.IsNullOrWhiteSpace(lastName))
            throw new ArgumentException("El apellido no puede estar vacío.", nameof(lastName));

        FirstName = firstName;
        LastName = lastName;
        Email = email;
        Phone = phone;
        IdentificationDocument = doc;
    }

    // Relaciones
    public virtual ICollection<Stay> Stays { get; private set; } = new List<Stay>();

    public virtual Tenant Tenant { get; private set; } = null!;
}
