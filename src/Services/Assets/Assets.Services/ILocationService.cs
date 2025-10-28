using Assets.Data.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Assets.Services;

public interface ILocationService
{
    Task<Location?> GetByIdAsync(Guid id);
    Task<IEnumerable<Location>> GetManyByIdAsync(params Guid[] ids);
    Task<IEnumerable<Location>> GetByParentIdAsync(Guid parentId);

    Task<Location> CreateAsync(Location location);
    Task<IEnumerable<Location>> CreateManyAsync(params Location[] locations);

    Task<Location?> UpdateAsync(Location location);
    Task<IEnumerable<Location>> UpdateManyAsync(params Location[] locations);
    Task<IEnumerable<Location>> ReparentChildrenAsync(Guid locationId, Guid? newParentId, params Guid[] childIds);

    Task DeleteAsync(Guid id);
    Task DeleteManyAsync(params Guid[] ids);
}
