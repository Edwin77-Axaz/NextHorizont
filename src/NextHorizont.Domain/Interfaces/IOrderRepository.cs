using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using NextHorizont.Domain.Entities;

namespace NextHorizont.Domain.Interfaces;

public interface IOrderRepository
{
    Task<Order?> GetByIdAsync(Guid id, Guid tenantId);
    Task<IEnumerable<Order>> GetActiveOrdersAsync(Guid tenantId);
    Task AddAsync(Order order);
    Task UpdateAsync(Order order);
}
