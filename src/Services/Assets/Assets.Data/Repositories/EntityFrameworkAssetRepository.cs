using Assets.Data.Context;
using Assets.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace Assets.Data.Repositories;

public class EntityFrameworkAssetRepository(AssetsDbContext context) : IAssetRepository
{
    public async Task<AssetEntity> AddAsync(AssetEntity asset)
        => (await AddManyAsync(asset)).First();

    public async Task<IEnumerable<AssetEntity>> AddManyAsync(params AssetEntity[] assets)
    {
        foreach (var asset in assets)
        {
            asset.AssetId = Guid.NewGuid();
            asset.CreatedAt = DateTime.UtcNow;
            asset.LastUpdatedAt = DateTime.UtcNow;

            context.Assets.Add(asset);
        }

        await context.SaveChangesAsync();

        return assets;
    }
    public async Task<IEnumerable<AssetEntity>> AddManyWithLocationAsync(params AssetEntity[] assets)
    {
        await AddManyAsync(assets);

        return await context.Assets
            .Include(a => a.Location)
            .Where(a => assets.Select(asset => asset.AssetId).Contains(a.AssetId))
            .ToListAsync();
    }

    public async Task<AssetEntity?> GetByIdAsync(Guid id)
        => await context.Assets.SingleOrDefaultAsync(a => a.AssetId == id);

    public async Task<AssetEntity?> GetByIdWithLocationAsync(Guid id)
            => await context.Assets.Include(a => a.Location).SingleOrDefaultAsync(a => a.AssetId == id);

    public async Task<IEnumerable<AssetEntity>> GetByLocationIdAsync(Guid id)
        => await context.Assets.Include(a => a.Location).Where(x => x.LocationId == id).ToListAsync();

    public async Task DeleteAsync(Guid id)
        => await DeleteManyAsync(id);

    public async Task DeleteManyAsync(params Guid[] ids)
    {
        foreach (var id in ids)
        {
            var asset = await context.Assets.FindAsync(id);
            if (asset != null)
            {
                context.Assets.Remove(asset);
            }
        }

        await context.SaveChangesAsync();
    }


    public async Task<AssetEntity?> UpdateAsync(AssetEntity asset)
        => (await UpdateManyAsync(asset)).FirstOrDefault();

    public async Task<IEnumerable<AssetEntity>> UpdateManyAsync(params AssetEntity[] assets)
    {
        foreach (var asset in assets)
        {
            asset.LastUpdatedAt = DateTime.UtcNow;
            context.Assets.Update(asset);
        }

        await context.SaveChangesAsync();

        return assets;
    }

    public async Task<IEnumerable<AssetEntity>> UpdateManyWithLocationAsync(params AssetEntity[] assets)
    {
        await UpdateManyAsync(assets);
        return await context.Assets
            .Include(a => a.Location)
            .Where(a => assets.Select(asset => asset.AssetId).Contains(a.AssetId))
            .ToListAsync();
    }
}
