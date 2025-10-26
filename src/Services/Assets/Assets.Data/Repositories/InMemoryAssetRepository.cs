using Assets.Data.Entities;

namespace Assets.Data.Repositories;

public class InMemoryAssetRepository : IAssetRepository
{
    private static readonly Dictionary<Guid, Asset> _assets = [];

    public async Task AddAssetAsync(Asset asset)
        => _assets[asset.Id] = asset;

    public async Task DeleteAssetAsync(Guid id) 
        => _assets.Remove(id);

    public async Task<IEnumerable<Asset>> GetAllAssetsAsync() 
        => _assets.Values.ToList().AsEnumerable();

    public async Task<Asset?> GetAssetByIdAsync(Guid id)
        => _assets.TryGetValue(id, out var asset) ? asset : null;

    public async Task<Asset?> UpdateAssetAsync(Asset asset)
    {
        _assets[asset.Id] = asset;
        return asset;
    }
}