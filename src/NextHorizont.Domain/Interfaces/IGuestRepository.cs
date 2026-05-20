using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using NextHorizont.Domain.Entities;

namespace NextHorizont.Domain.Interfaces;

public interface IGuestRepository
{
    Task<Guest?> GetByIdAsync(Guid id, Guid tenantId);
    Task<Guest?> GetByDocumentAsync(string identificationDocument, Guid tenantId);
    Task<IEnumerable<Guest>> SearchAsync(string searchTerm, Guid tenantId);
    Task AddAsync(Guest guest);
    Task UpdateAsync(Guest guest);
}
