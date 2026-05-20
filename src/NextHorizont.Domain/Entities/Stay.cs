using System;
using NextHorizont.Domain.Enums;
using NextHorizont.Domain.Exceptions;

namespace NextHorizont.Domain.Entities;

public partial class Stay
{
    // Constructor para EF Core
    protected Stay()
    {
    }

    // Constructor de Dominio
    public Stay(Guid id, Guid tenantId, Guid locationId, Guid guestId, DateOnly checkInDate, DateOnly checkOutDate, decimal nightlyRate)
    {
        if (checkOutDate <= checkInDate)
            throw new StayValidationException("La fecha de salida debe ser posterior a la fecha de ingreso.");
        if (nightlyRate < 0)
            throw new ArgumentOutOfRangeException(nameof(nightlyRate), "La tarifa por noche no puede ser negativa.");

        Id = id;
        TenantId = tenantId;
        LocationId = locationId;
        GuestId = guestId;
        CheckInDate = checkInDate;
        CheckOutDate = checkOutDate;
        Status = StayStatus.Reservada;
        NightlyRate = nightlyRate;
        CreatedAt = DateTime.UtcNow;
        UpdatedAt = DateTime.UtcNow;
    }

    public Guid Id { get; private set; }

    public Guid TenantId { get; private set; }

    public Guid LocationId { get; private set; }

    public Guid GuestId { get; private set; }

    public DateOnly CheckInDate { get; private set; }

    public DateOnly CheckOutDate { get; private set; }

    public StayStatus Status { get; private set; }

    public decimal NightlyRate { get; private set; }

    public Guid? MasterOrderId { get; private set; }

    public DateTime? CreatedAt { get; private set; }

    public DateTime? UpdatedAt { get; private set; }

    // Métodos de Negocio
    public void CheckIn()
    {
        if (Status != StayStatus.Reservada)
            throw new StayValidationException($"No se puede realizar Check-In para una estadía con estado {Status}.");

        Status = StayStatus.CheckIn;
        Location.ChangeRoomStatus(RoomStatus.Ocupada);
        UpdatedAt = DateTime.UtcNow;
    }

    public void LinkMasterOrder(Order order)
    {
        if (Status != StayStatus.CheckIn)
            throw new StayValidationException("Solo se puede vincular una orden de cuenta maestra a estadías activas (Checked-In).");

        MasterOrderId = order.Id;
        UpdatedAt = DateTime.UtcNow;
    }

    public void CheckOut()
    {
        if (Status != StayStatus.CheckIn)
            throw new StayValidationException($"Solo se puede realizar Check-Out para estadías con estado Check-In. Estado actual: {Status}.");

        // Validar que la cuenta maestra esté pagada
        if (MasterOrder != null && MasterOrder.Status != OrderStatus.Pagada && MasterOrder.Status != OrderStatus.Cancelada)
        {
            throw new StayValidationException("No se puede realizar el Check-Out. La orden maestra de consumos no ha sido saldada o cancelada.");
        }

        Status = StayStatus.CheckOut;
        Location.ChangeRoomStatus(RoomStatus.Sucia); // Requiere limpieza antes de estar Disponible
        UpdatedAt = DateTime.UtcNow;
    }

    public void Cancel()
    {
        if (Status != StayStatus.Reservada)
            throw new StayValidationException("Solo se pueden cancelar reservas de estadías pendientes.");

        Status = StayStatus.Cancelada;
        Location.ChangeRoomStatus(RoomStatus.Disponible);
        UpdatedAt = DateTime.UtcNow;
    }

    public void ChangeDates(DateOnly newCheckIn, DateOnly newCheckOut)
    {
        if (Status == StayStatus.CheckOut || Status == StayStatus.Cancelada)
            throw new StayValidationException("No se pueden cambiar las fechas de estadías finalizadas o canceladas.");

        if (newCheckOut <= newCheckIn)
            throw new StayValidationException("La fecha de salida debe ser posterior a la fecha de ingreso.");

        CheckInDate = newCheckIn;
        CheckOutDate = newCheckOut;
        UpdatedAt = DateTime.UtcNow;
    }

    public void UpdateNightlyRate(decimal newRate)
    {
        if (newRate < 0)
            throw new ArgumentOutOfRangeException(nameof(newRate), "La tarifa por noche no puede ser negativa.");

        NightlyRate = newRate;
        UpdatedAt = DateTime.UtcNow;
    }

    // Relaciones
    public virtual Guest Guest { get; private set; } = null!;

    public virtual Location Location { get; private set; } = null!;

    public virtual Order? MasterOrder { get; private set; }

    public virtual Tenant Tenant { get; private set; } = null!;
}
