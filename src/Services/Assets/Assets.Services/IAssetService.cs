using Assets.Data.Entities;
using Assets.Services.Models;

namespace Assets.Services;

public interface IAssetService
{
    Task<AssetWithLocation> CreateAsync(Asset asset);
    Task<IEnumerable<AssetWithLocation>> CreateManyAsync(params Asset[] assets);

    Task<AssetWithLocation?> GetByIdAsync(Guid id);
    Task<LocationWithAssets?> GetByLocationIdAsync(Guid locationId);

    Task<AssetWithLocation?> UpdateAsync(Asset assets);
    Task<IEnumerable<AssetWithLocation>> UpdateManyAsync(params Asset[] assets);

    Task DeleteAsync(Guid id);
    Task DeleteManyAsync(params Guid[] ids);
}
