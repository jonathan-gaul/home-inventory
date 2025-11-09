using Assets.Data.Context;
using Assets.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace Assets.Data.Repositories;

public class EntityFrameworkLocationRepository(AssetsDbContext context) : ILocationRepository
{
    public async Task<LocationEntity> AddAsync(LocationEntity location)
        => (await AddManyAsync(location)).First();

    public async Task<IEnumerable<LocationEntity>> AddManyAsync(params LocationEntity[] locations)
    {
        foreach (var location in locations)
        {
            location.LocationId = Guid.NewGuid();
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

    public async Task<LocationEntity?> GetByIdAsync(Guid id)
        => await context.Locations.SingleOrDefaultAsync(l => l.LocationId == id);

    public async Task<LocationEntity?> GetByIdWithAssetsAsync(Guid id)
        => await context.Locations
            .Include(l => l.Assets)
            .SingleOrDefaultAsync(l => l.LocationId == id);

    public async Task<LocationEntity?> GetByIdWithChildrenAsync(Guid id)
        => await context.Locations
            .Include(l => l.ChildLocations)
            .SingleOrDefaultAsync(l => l.LocationId == id);

    public async Task<IEnumerable<LocationEntity>> GetManyByIdAsync(params Guid[] ids)
        => await context.Locations.Where(x => ids.Contains(x.LocationId)).ToListAsync();

    public async Task<IEnumerable<LocationEntity>> GetManyByIdWithAssetsAsync(params Guid[] ids)
        => await context.Locations
            .Include(l => l.Assets)
            .Where(x => ids.Contains(x.LocationId))
            .ToListAsync();

    public async Task<LocationEntity?> UpdateAsync(LocationEntity location)
        => (await UpdateManyAsync(location)).FirstOrDefault();

    public async Task<IEnumerable<LocationEntity>> UpdateManyAsync(params LocationEntity[] locations)
    {
        foreach (var location in locations)
        {
            location.LastUpdatedAt = DateTime.UtcNow;
            context.Locations.Update(location);
        }

        await context.SaveChangesAsync();

        return locations;
    }

    public async Task<IEnumerable<LocationEntity>> GetByParentIdAsync(Guid parentId)
        => await context.Locations.Where(x => x.ParentLocationId == parentId).ToListAsync();

    public async Task<IEnumerable<LocationEntity>> GetByParentIdWithAssetsAsync(Guid parentId)
        => await context.Locations
            .Include(l => l.Assets)
            .Where(x => x.ParentLocationId == parentId)
            .ToListAsync();
}
