using Assets.Data.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Assets.Data.Repositories;

public interface ILocationRepository
{
    Task<Location?> GetLocationByIdAsync(Guid id);
    Task<IEnumerable<Location>> GetLocationsByIdAsync(params Guid[] ids);
    Task AddLocationAsync(Location location);
    Task<Location?> UpdateLocationAsync(Location location);
    Task<IEnumerable<Location>> UpdateLocationsAsync(params Location[] locations);
    Task DeleteLocationAsync(Guid id);
    Task<IEnumerable<Location>> GetLocationsByParentIdAsync(Guid id);
}
