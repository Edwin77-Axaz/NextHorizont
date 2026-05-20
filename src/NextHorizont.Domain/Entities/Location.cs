using System;
using System.Collections.Generic;
using NextHorizont.Domain.Enums;

namespace NextHorizont.Domain.Entities;

public partial class Location
{
    // Constructor para EF Core
    protected Location()
    {
    }

    // Constructor de Dominio
    public Location(Guid id, Guid tenantId, Guid areaId, string name, LocationType type, int? capacity)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("El nombre de la ubicación no puede estar vacío.", nameof(name));

        Id = id;
        TenantId = tenantId;
        AreaId = areaId;
        Name = name;
        Type = type;
        Capacity = capacity ?? 4;
        CreatedAt = DateTime.UtcNow;

        if (type == LocationType.Habitacion)
        {
            RoomStatus = Enums.RoomStatus.Disponible;
        }
    }

    public Guid Id { get; private set; }

    public Guid TenantId { get; private set; }

    public Guid AreaId { get; private set; }

    public string Name { get; private set; } = null!;

    public LocationType Type { get; private set; }

    public int? Capacity { get; private set; }

    public RoomStatus? RoomStatus { get; private set; }

    public string? RoomCategory { get; private set; }

    public decimal? RoomRatePerNight { get; private set; }

    public DateTime? CreatedAt { get; private set; }

    // Métodos de Negocio
    public void ChangeRoomStatus(RoomStatus newStatus)
    {
        if (Type != LocationType.Habitacion)
            throw new InvalidOperationException("Solo se puede cambiar el estado de habitación a ubicaciones de tipo Habitación.");

        RoomStatus = newStatus;
    }

    public void ConfigureAsRoom(string category, decimal ratePerNight)
    {
        if (Type != LocationType.Habitacion)
            throw new InvalidOperationException("Solo se pueden configurar datos de hotel en ubicaciones de tipo Habitación.");
        if (ratePerNight < 0)
            throw new ArgumentOutOfRangeException(nameof(ratePerNight), "La tarifa por noche no puede ser negativa.");

        RoomCategory = category;
        RoomRatePerNight = ratePerNight;
    }

    public void UpdateDetails(string name, int capacity)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("El nombre no puede estar vacío.", nameof(name));
        if (capacity <= 0)
            throw new ArgumentOutOfRangeException(nameof(capacity), "La capacidad debe ser mayor a cero.");

        Name = name;
        Capacity = capacity;
    }

    // Relaciones
    public virtual Area Area { get; private set; } = null!;

    public virtual ICollection<Order> Orders { get; private set; } = new List<Order>();

    public virtual ICollection<Stay> Stays { get; private set; } = new List<Stay>();

    public virtual Tenant Tenant { get; private set; } = null!;
}
