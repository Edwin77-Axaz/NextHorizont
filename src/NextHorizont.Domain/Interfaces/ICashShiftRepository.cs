using System;
using System.Threading.Tasks;
using NextHorizont.Domain.Entities;

namespace NextHorizont.Domain.Interfaces;

public interface ICashShiftRepository
{
    Task<CashShift?> GetActiveShiftAsync(Guid tenantId, Guid userId);
    Task AddAsync(CashShift shift);
    Task UpdateAsync(CashShift shift);
}
