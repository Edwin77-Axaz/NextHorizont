using System;
using System.Collections.Generic;
using NextHorizont.Domain.Enums;
using NextHorizont.Domain.Exceptions;

namespace NextHorizont.Domain.Entities;

public partial class CashShift
{
    // Constructor para EF Core
    protected CashShift()
    {
    }

    // Constructor de Dominio
    public CashShift(Guid id, Guid tenantId, Guid userId, decimal openingBalance)
    {
        if (openingBalance < 0)
            throw new ArgumentOutOfRangeException(nameof(openingBalance), "El saldo de apertura no puede ser negativo.");

        Id = id;
        TenantId = tenantId;
        UserId = userId;
        OpeningTime = DateTime.UtcNow;
        OpeningBalance = openingBalance;
        Status = CashShiftStatus.Abierto;
    }

    public Guid Id { get; private set; }

    public Guid TenantId { get; private set; }

    public Guid UserId { get; private set; }

    public DateTime OpeningTime { get; private set; }

    public DateTime? ClosingTime { get; private set; }

    public decimal OpeningBalance { get; private set; }

    public decimal? ClosingBalance { get; private set; }

    public string? DenominationsClosing { get; private set; }

    public CashShiftStatus Status { get; private set; }

    // Métodos de Negocio
    public void Close(decimal closingBalance, string? denominationsClosing = null)
    {
        if (Status == CashShiftStatus.Cerrado)
            throw new CashShiftClosedException("Este turno de caja ya ha sido cerrado previamente.");
        if (closingBalance < 0)
            throw new ArgumentOutOfRangeException(nameof(closingBalance), "El saldo de cierre no puede ser negativo.");

        ClosingTime = DateTime.UtcNow;
        ClosingBalance = closingBalance;
        DenominationsClosing = denominationsClosing;
        Status = CashShiftStatus.Cerrado;
    }

    // Relaciones
    public virtual ICollection<Payment> Payments { get; private set; } = new List<Payment>();

    public virtual Tenant Tenant { get; private set; } = null!;

    public virtual User User { get; private set; } = null!;
}
