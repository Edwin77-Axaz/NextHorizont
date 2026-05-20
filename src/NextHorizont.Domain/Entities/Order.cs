using System;
using System.Collections.Generic;
using System.Linq;
using NextHorizont.Domain.Enums;
using NextHorizont.Domain.Exceptions;

namespace NextHorizont.Domain.Entities;

public partial class Order
{
    // Constructor para EF Core
    protected Order()
    {
    }

    // Constructor de Dominio
    public Order(Guid id, Guid tenantId, Guid? locationId, Guid? userId, OrderOrigin origin)
    {
        Id = id;
        TenantId = tenantId;
        LocationId = locationId;
        UserId = userId;
        Status = OrderStatus.Abierta;
        Origin = origin;
        Total = 0m;
        TipAmount = 0m;
        CreatedAt = DateTime.UtcNow;
        UpdatedAt = DateTime.UtcNow;
    }

    public Guid Id { get; private set; }

    public Guid TenantId { get; private set; }

    public Guid? LocationId { get; private set; }

    public Guid? UserId { get; private set; }

    public OrderStatus Status { get; private set; }

    public OrderOrigin Origin { get; private set; }

    public decimal? Total { get; private set; }

    public decimal? TipAmount { get; private set; }

    public DateTime? CreatedAt { get; private set; }

    public DateTime? UpdatedAt { get; private set; }

    // Métodos de Negocio
    public void AddProduct(Product product, int quantity)
    {
        if (Status != OrderStatus.Abierta && Status != OrderStatus.EnviadaACocina)
            throw new InvalidOrderStateException($"No se pueden agregar productos a una orden en estado {Status}.");

        if (quantity <= 0)
            throw new ArgumentException("La cantidad debe ser mayor a cero.", nameof(quantity));

        var existingLine = OrderLines.FirstOrDefault(ol => ol.ProductId == product.Id);
        if (existingLine != null)
        {
            existingLine.UpdateQuantity(existingLine.Quantity + quantity);
        }
        else
        {
            var line = new OrderLine(Guid.NewGuid(), TenantId, Id, product.Id, quantity, product.Price);
            OrderLines.Add(line);
        }

        CalculateTotal();
        UpdatedAt = DateTime.UtcNow;
    }

    public void RemoveLine(Guid lineId)
    {
        if (Status != OrderStatus.Abierta)
            throw new InvalidOrderStateException("Solo se pueden eliminar productos de órdenes abiertas.");

        var line = OrderLines.FirstOrDefault(ol => ol.Id == lineId);
        if (line != null)
        {
            OrderLines.Remove(line);
            CalculateTotal();
            UpdatedAt = DateTime.UtcNow;
        }
    }

    public void SendToKitchen()
    {
        if (Status != OrderStatus.Abierta)
            throw new InvalidOrderStateException("Solo se pueden enviar a cocina órdenes que estén abiertas.");

        if (!OrderLines.Any())
            throw new DomainException("No se puede enviar a cocina una orden sin productos.");

        Status = OrderStatus.EnviadaACocina;
        UpdatedAt = DateTime.UtcNow;
    }

    public void MarkAsEntregada()
    {
        if (Status != OrderStatus.EnviadaACocina && Status != OrderStatus.Abierta)
            throw new InvalidOrderStateException("La orden debe estar en cocina o abierta para ser entregada.");

        Status = OrderStatus.Entregada;
        UpdatedAt = DateTime.UtcNow;
    }

    public void ApplyPayment(Guid paymentId, decimal amount, PaymentMethod method, CashShift cashShift, string? referenceCode = null)
    {
        if (Status == OrderStatus.Pagada || Status == OrderStatus.Cancelada)
            throw new InvalidOrderStateException("No se pueden procesar pagos para órdenes finalizadas o canceladas.");

        if (cashShift.Status != CashShiftStatus.Abierto)
            throw new CashShiftClosedException("La caja diaria debe estar abierta para recibir pagos.");

        if (amount <= 0)
            throw new ArgumentException("El monto del pago debe ser mayor a cero.", nameof(amount));

        if (method.RequiresReference && string.IsNullOrWhiteSpace(referenceCode))
            throw new DomainException($"El método de pago {method.Name} requiere un código de referencia.");

        var payment = new Payment(paymentId, TenantId, Id, method.Id, cashShift.Id, amount, referenceCode);
        Payments.Add(payment);

        var currentPaidAmount = Payments.Sum(p => p.Amount);
        var orderTotal = Total ?? 0m;

        if (currentPaidAmount >= orderTotal)
        {
            Status = OrderStatus.Pagada;
        }

        UpdatedAt = DateTime.UtcNow;
    }

    public void Cancel(string reason)
    {
        if (Status == OrderStatus.Pagada)
            throw new InvalidOrderStateException("No se puede cancelar una orden que ya ha sido pagada.");

        if (Status == OrderStatus.Cancelada)
            return;

        Status = OrderStatus.Cancelada;
        UpdatedAt = DateTime.UtcNow;
    }

    private void CalculateTotal()
    {
        Total = OrderLines.Sum(ol => ol.Subtotal);
    }

    // Relaciones
    public virtual Location? Location { get; private set; }

    public virtual ICollection<OrderLine> OrderLines { get; private set; } = new List<OrderLine>();

    public virtual ICollection<Payment> Payments { get; private set; } = new List<Payment>();

    public virtual ICollection<Stay> Stays { get; private set; } = new List<Stay>();

    public virtual Tenant Tenant { get; private set; } = null!;

    public virtual User? User { get; private set; }
}
