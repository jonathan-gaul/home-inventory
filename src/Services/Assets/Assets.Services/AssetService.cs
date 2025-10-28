using Assets.Data.Entities;
using Assets.Data.Repositories;
using Assets.Services.Models;

namespace Assets.Services;

public class AssetService(IAssetRepository assetRepository, ILocationService locationService) 
    : IAssetService
{
    public async Task<AssetWithLocation> CreateAsync(Asset asset)
    {
        ArgumentNullException.ThrowIfNull(asset);

        if (asset.LocationId == Guid.Empty)
            throw new ArgumentException("Asset must have a valid LocationId.");

        var location = await locationService.GetByIdAsync(asset.LocationId) 
            ?? throw new ArgumentException($"Location with Id {asset.LocationId} does not exist.");

        await assetRepository.AddAsync(asset);

        return new(asset, location);
    }
        

    public async Task<IEnumerable<AssetWithLocation>> CreateManyAsync(params Asset[] assets)
    {
        ArgumentNullException.ThrowIfNull(assets);

        // Build a map of LocationId to Location to minimize calls to locationService
        var locationCache = new Dictionary<Guid, Location>();
        var results = new List<AssetWithLocation>();

        foreach (var asset in await assetRepository.AddManyAsync(assets))
        {
            if (!locationCache.TryGetValue(asset.LocationId, out var location))
            {
                location = await locationService.GetByIdAsync(asset.LocationId)
                    ?? throw new ArgumentException($"Location with Id {asset.LocationId} does not exist.");

                locationCache[asset.LocationId] = location;
            }

            results.Add(new(asset, location));
        }

        return results;
    }

    public async Task<AssetWithLocation?> GetByIdAsync(Guid id)
    {
        var asset = await assetRepository.GetByIdAsync(id);
        if (asset is null)
            return null;

        var location = await locationService.GetByIdAsync(asset.LocationId);            

        return new(asset, location);
    }

    public async Task<LocationWithAssets?> GetByLocationIdAsync(Guid locationId)
    {
        var location = await locationService.GetByIdAsync(locationId);
        if (location is null)
            return null;

        var assets = await assetRepository.GetByLocationIdAsync(locationId);        

        return new(location, assets);
    }
    
    public async Task<AssetWithLocation?> UpdateAsync(Asset asset)
    {
        var updatedAsset = await assetRepository.UpdateAsync(asset);
        if (updatedAsset is null)
            return null;

        var location = await locationService.GetByIdAsync(updatedAsset.LocationId);

        return new(updatedAsset, location);
    }
        

    public async Task<IEnumerable<AssetWithLocation>> UpdateManyAsync(params Asset[] assets)
    {
        var updatedAssets = await assetRepository.UpdateManyAsync(assets);
        var results = new List<AssetWithLocation>();

        var locationCache = new Dictionary<Guid, Location?>();

        foreach (var asset in updatedAssets)
        {
            if (!locationCache.TryGetValue(asset.LocationId, out var location))
            {
                location = await locationService.GetByIdAsync(asset.LocationId);                
                locationCache[asset.LocationId] = location;
            }
            
            results.Add(new(asset, location));
        }
        
        return results;
    }

    public async Task DeleteAsync(Guid id)
        => await assetRepository.DeleteAsync(id);

    public async Task DeleteManyAsync(params Guid[] ids)
        => await assetRepository.DeleteManyAsync(ids);
}
