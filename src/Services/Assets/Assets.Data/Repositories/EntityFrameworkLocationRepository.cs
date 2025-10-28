using Assets.Data.Context;
using Assets.Data.Entities;
using Assets.Data.Migrations;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Assets.Data.Repositories;

public class EntityFrameworkLocationRepository(AssetsDbContext context) : ILocationRepository
{
    public async Task<Location> AddAsync(Location location) 
        => (await AddManyAsync(location)).First();

    public async Task<IEnumerable<Location>> AddManyAsync(params Location[] locations)
    {
        foreach (var location in locations)
        {
            location.Id = Guid.NewGuid();
            location.CreatedAt = DateTime.UtcNow;
            location.LastUpdatedAt = DateTime.UtcNow;

            context.Locations.Add(location);
        }

        await context.SaveChangesAsync();
        return locations;
    }

    public async Task DeleteAsync(Guid id) 
        => await DeleteManyAsync(id);

    public async Task DeleteManyAsync(params Guid[] ids)
    {
        foreach (var id in ids)
        {
            var location = await context.Locations.FindAsync(id);
            if (location != null)
            {
                context.Locations.Remove(location);
            }
        }

        await context.SaveChangesAsync();
    }
    
    public async Task<Location?> GetByIdAsync(Guid id)
        => await context.Locations.FindAsync(id);

    public async Task<IEnumerable<Location>> GetManyByIdAsync(params Guid[] ids)
        => await context.Locations.Where(x => ids.Contains(x.Id)).ToListAsync();

    public async Task<Location?> UpdateAsync(Location location)
        => (await UpdateManyAsync(location)).FirstOrDefault();

    public async Task<IEnumerable<Location>> UpdateManyAsync(params Location[] locations)
    {
        foreach (var location in locations)
        {
            location.LastUpdatedAt = DateTime.UtcNow;
            context.Locations.Update(location);
        }

        await context.SaveChangesAsync();

        return locations;
    }

    public async Task<IEnumerable<Location>> GetByParentIdAsync(Guid parentId)
        => await context.Locations.Where(x => x.ParentLocationId == parentId).ToListAsync();
}
