using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using NextHorizont.Domain.Entities;
using NextHorizont.Domain.Enums;

namespace NextHorizont.Domain.Interfaces;

public interface ILocationRepository
{
    Task<Location?> GetByIdAsync(Guid id, Guid tenantId);
    Task<IEnumerable<Location>> GetByAreaAsync(Guid areaId, Guid tenantId);
    Task<IEnumerable<Location>> GetByRoomStatusAsync(RoomStatus status, Guid tenantId);
    Task<IEnumerable<Location>> GetByTypeAsync(LocationType type, Guid tenantId);
    Task AddAsync(Location location);
    Task UpdateAsync(Location location);
}
