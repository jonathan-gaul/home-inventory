using Assets.Data.Context;
using Assets.Data.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Assets.Data.Repositories;

public class EntityFrameworkLocationRepository(AssetsDbContext context) : ILocationRepository
{
    public async Task AddLocationAsync(Location location)
    {
        location.Id = Guid.NewGuid();
        location.CreatedAt = DateTime.UtcNow;
        location.LastUpdatedAt = DateTime.UtcNow;

        context.Locations.Add(location);
        await context.SaveChangesAsync();
    }

    public async Task DeleteLocationAsync(Guid id)
    {
        var location = await context.Locations.FindAsync(id);
        if (location != null)
        {
            context.Locations.Remove(location);
            await context.SaveChangesAsync();
        }
    }
    
    public async Task<Location?> GetLocationByIdAsync(Guid id)
        => await context.Locations.FindAsync(id);

    public async Task<IEnumerable<Location>> GetLocationsByIdAsync(params Guid[] ids)
        => await context.Locations.Where(x => ids.Contains(x.Id)).ToListAsync();

    public async Task<Location?> UpdateLocationAsync(Location location)
    {
        location.LastUpdatedAt = DateTime.UtcNow;
        context.Locations.Update(location);
        await context.SaveChangesAsync();

        return location;
    }

    public async Task<IEnumerable<Location>> UpdateLocationsAsync(params Location[] locations)
    {
        foreach (var location in locations)
        {
            location.LastUpdatedAt = DateTime.UtcNow;
            context.Locations.Update(location);
        }

        await context.SaveChangesAsync();

        return locations;
    }

    public async Task<IEnumerable<Location>> GetLocationsByParentIdAsync(Guid parentId)
        => await context.Locations.Where(x => x.ParentLocationId == parentId).ToListAsync();
}
