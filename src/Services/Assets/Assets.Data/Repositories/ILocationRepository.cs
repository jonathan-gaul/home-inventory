using Assets.Data.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Assets.Data.Repositories;

public interface ILocationRepository
{
    Task<Location?> GetByIdAsync(Guid id);
    Task<IEnumerable<Location>> GetManyByIdAsync(params Guid[] ids);
    Task<IEnumerable<Location>> GetByParentIdAsync(Guid id);

    Task<Location> AddAsync(Location location);
    Task<IEnumerable<Location>> AddManyAsync(params Location[] locations);

    Task<Location?> UpdateAsync(Location location);
    Task<IEnumerable<Location>> UpdateManyAsync(params Location[] locations);    

    Task DeleteAsync(Guid id);
    Task DeleteManyAsync(params Guid[] ids);    
}
