using Assets.Data.Repositories;
using Assets.Services.Models;

namespace Assets.Services;

public class LocationService(ILocationRepository locationRepository) : ILocationService
{
    public async Task<Location?> GetByIdAsync(Guid id)
    {
        var entity = await locationRepository.GetByIdAsync(id);
        return entity is null ? null : Location.FromEntity(entity);
    }

    public async Task<LocationWithAssets?> GetByIdWithAssetsAsync(Guid id)
    {
        var entity = await locationRepository.GetByIdWithAssetsAsync(id);
        return entity is null ? null : LocationWithAssets.FromEntity(entity);
    }

    public async Task<IEnumerable<Location>> GetByParentIdAsync(Guid parentId)
        => (await locationRepository.GetByParentIdAsync(parentId)).Select(Location.FromEntity);

    public async Task<IEnumerable<Location>> GetManyByIdAsync(params Guid[] ids)
        => (await locationRepository.GetManyByIdAsync(ids)).Select(Location.FromEntity);


    public async Task<Location?> CreateAsync(Location location)
    {
        var addedEntity = await locationRepository.AddAsync(location.ToEntity());
        return Location.FromEntity(addedEntity);
    }

    public async Task<IEnumerable<Location>> CreateManyAsync(params Location[] locations)
    {
        var locationEntities = locations.Select(l => l.ToEntity()).ToArray();
        return (await locationRepository.AddManyAsync(locationEntities)).Select(Location.FromEntity);
    }

    public async Task<Location?> UpdateAsync(Location location)
    {
        var updatedEntity = await locationRepository.UpdateAsync(location.ToEntity());
        if (updatedEntity is null)
            return null;

        return Location.FromEntity(updatedEntity);
    }

    public async Task<IEnumerable<Location>> UpdateManyAsync(params Location[] locations)
    {
        var locationEntities = locations.Select(l => l.ToEntity()).ToArray();
        return (await locationRepository.UpdateManyAsync(locationEntities)).Select(Location.FromEntity);
    }


    public Task DeleteAsync(Guid id)
        => locationRepository.DeleteAsync(id);

    public Task DeleteManyAsync(params Guid[] ids)
        => locationRepository.DeleteManyAsync(ids);

    public async Task<IEnumerable<Location>> ReparentChildrenAsync(Guid parentId, Guid? newParentId, params Guid[] childIds)
    {
        var parentLocation = await locationRepository.GetByIdWithChildrenAsync(parentId);

        if (parentLocation is null)
            throw new ArgumentException($"Parent location does not exist: {parentId}");

        var existingIds = parentLocation.ChildLocations.Select(x => x.LocationId);
        var childIdsToAdd = childIds.Except(existingIds);
        var newChildren = await locationRepository.GetManyByIdAsync([.. childIdsToAdd]);

        var childIdsThatDontExist = childIds.Where(x => !newChildren.Select(c => c.LocationId).Contains(x));

        if (childIdsThatDontExist.Any())
        {
            var idsText = string.Join(", ", childIdsThatDontExist);
            throw new ArgumentException($"The following children do not exist: {idsText}");
        }

        var childIdsToRemove = existingIds.Except(childIds);

        // Children removed from this location will be moved to the new parent if possible. 
        // Otherwise, return an error.
        if (childIdsToRemove.Any())
        {
            if (newParentId is null)
            {
                var idsText = string.Join(", ", childIdsToRemove);
                throw new ArgumentException($"Cannot remove the following children as no new parent was provided: {idsText}");
            }

            var newParentLocation = await locationRepository.GetByIdAsync(newParentId.Value);
            if (newParentLocation is null)
            {
                var idsText = string.Join(", ", childIdsToRemove);
                throw new ArgumentException($"Cannot remove the following children as the new parent does not exist: {idsText}");
            }

            var childrenToRemove = await locationRepository.GetManyByIdAsync([.. childIdsToRemove]);

            // We won't error on children that don't exist here in case the caller is trying to remove them.
            foreach (var child in childrenToRemove)
                child.ParentLocationId = newParentId;

            await locationRepository.UpdateManyAsync([.. childrenToRemove]);
        }

        // Set parent of new children to this location.
        if (childIdsToAdd.Any())
        {
            var childrenToAdd = await locationRepository.GetManyByIdAsync([.. childIdsToAdd]);

            foreach (var child in childrenToAdd)
                child.ParentLocationId = parentId;

            await locationRepository.UpdateManyAsync([.. childrenToAdd]);
        }

        return (await locationRepository.GetByParentIdAsync(parentId)).Select(Location.FromEntity);
    }
}
