using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using NextHorizont.Domain.Entities;

namespace NextHorizont.Domain.Interfaces;

public interface IStayRepository
{
    Task<Stay?> GetByIdAsync(Guid id, Guid tenantId);
    Task<IEnumerable<Stay>> GetActiveStaysAsync(Guid tenantId);
    Task AddAsync(Stay stay);
    Task UpdateAsync(Stay stay);
}
