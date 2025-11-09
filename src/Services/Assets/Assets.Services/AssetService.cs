using Assets.Data.Entities;
using Assets.Data.Repositories;
using Assets.Services.Models;

namespace Assets.Services;

public class AssetService(IAssetRepository assetRepository)
    : IAssetService
{
    public async Task<AssetWithLocation> CreateAsync(Asset asset)
    {
        ArgumentNullException.ThrowIfNull(asset);

        if (asset.LocationId == Guid.Empty)
            throw new ArgumentException("Asset must have a valid LocationId.");

        var assetEntity = asset.ToEntity();

        return AssetWithLocation.FromEntity(await assetRepository.AddAsync(assetEntity))!;
    }


    public async Task<IEnumerable<AssetWithLocation>> CreateManyAsync(params Asset[] assets)
    {
        ArgumentNullException.ThrowIfNull(assets);

        var entities = assets.Select(a => a.ToEntity()).ToArray();
        var addedEntities = await assetRepository.AddManyWithLocationAsync(entities);
        return addedEntities is null
            ? throw new Exception("Failed to create assets - repository returned null.")
            : [.. addedEntities.OfType<AssetEntity>().Select(AssetWithLocation.FromEntity)];
    }

    public async Task<AssetWithLocation?> GetByIdAsync(Guid id)
    {
        if (id == Guid.Empty)
            return null;

        var assetEntity = await assetRepository.GetByIdWithLocationAsync(id);

        return assetEntity is null 
            ? null
            : AssetWithLocation.FromEntity(assetEntity);
    }

    public async Task<AssetWithLocation?> UpdateAsync(Asset asset)
    {
        var assetEntity = asset.ToEntity();

        var updatedAsset = await assetRepository.UpdateAsync(assetEntity);
        if (updatedAsset is null)
            return null;

        return AssetWithLocation.FromEntity(updatedAsset);
    }


    public async Task<IEnumerable<AssetWithLocation>> UpdateManyAsync(params Asset[] assets)
    {
        var assetEntities = assets.Select(a => a.ToEntity()).ToArray();

        var updatedAssets = await assetRepository.UpdateManyWithLocationAsync(assetEntities);

        return [.. updatedAssets.OfType<AssetEntity>().Select(AssetWithLocation.FromEntity)];
    }

    public async Task DeleteAsync(Guid id)
        => await assetRepository.DeleteAsync(id);

    public async Task DeleteManyAsync(params Guid[] ids)
        => await assetRepository.DeleteManyAsync(ids);
}
