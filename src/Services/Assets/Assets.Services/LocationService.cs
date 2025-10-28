using Assets.Data.Entities;
using Assets.Data.Repositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace Assets.Services;

public class LocationService(ILocationRepository locationRepository) : ILocationService
{
    public Task<Location?> GetByIdAsync(Guid id)
        => locationRepository.GetByIdAsync(id);
    
    public Task<IEnumerable<Location>> GetByParentIdAsync(Guid parentId)
        => locationRepository.GetByParentIdAsync(parentId);

    public Task<IEnumerable<Location>> GetManyByIdAsync(params Guid[] ids)
        => locationRepository.GetManyByIdAsync(ids);


    public Task<Location> CreateAsync(Location location)
        => locationRepository.AddAsync(location);
    
    public Task<IEnumerable<Location>> CreateManyAsync(params Location[] locations)
        => locationRepository.AddManyAsync(locations);


    public Task<Location?> UpdateAsync(Location location)
        => locationRepository.UpdateAsync(location);

    public Task<IEnumerable<Location>> UpdateManyAsync(params Location[] locations)
        => locationRepository.UpdateManyAsync(locations);


    public Task DeleteAsync(Guid id)
        => locationRepository.DeleteAsync(id);
    public Task DeleteManyAsync(params Guid[] ids)
        => locationRepository.DeleteManyAsync(ids);

    public async Task<IEnumerable<Location>> ReparentChildrenAsync(Guid parentId, Guid? newParentId, params Guid[] childIds)
    {        
        var existingChildren = await locationRepository.GetByParentIdAsync(parentId);

        var existingIds = existingChildren.Select(x => x.Id);
        var childIdsToAdd = childIds.Except(existingIds);
        var newChildren = await locationRepository.GetManyByIdAsync([.. childIdsToAdd]);

        var childIdsThatDontExist = childIds.Where(x => !newChildren.Select(c => c.Id).Contains(x));

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

            var parentLocation = await locationRepository.GetByIdAsync(newParentId.Value);
            if (parentLocation is null)
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

        return await locationRepository.GetByParentIdAsync(parentId);
    }
}
