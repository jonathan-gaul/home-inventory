using Assets.Data.Context;
using Assets.Data.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Assets.Data.Repositories;

public class EntityFrameworkAssetRepository(AssetsDbContext context) : IAssetRepository
{
    public async Task AddAssetAsync(Asset asset)
    {
        asset.Id = Guid.NewGuid();
        asset.CreatedAt = DateTime.UtcNow;
        asset.LastUpdatedAt = DateTime.UtcNow;

        context.Assets.Add(asset);
        await context.SaveChangesAsync().ConfigureAwait(false);
    }

    public async Task DeleteAssetAsync(Guid id)
    {
        var asset = await context.Assets.FindAsync(id);
        if (asset != null)
        {
            context.Assets.Remove(asset);
            await context.SaveChangesAsync().ConfigureAwait(false);
        }
    }

    public async Task<IEnumerable<Asset>> GetAllAssetsAsync()
        => (await context.Assets.ToListAsync().ConfigureAwait(false)).AsEnumerable();

    public async Task<Asset?> GetAssetByIdAsync(Guid id)
        => await context.Assets.FindAsync(id).ConfigureAwait(false);

    public async Task<Asset?> UpdateAssetAsync(Asset asset)
    {
        asset.LastUpdatedAt = DateTime.UtcNow;
        context.Assets.Update(asset);
        await context.SaveChangesAsync().ConfigureAwait(false);

        return asset;
    }
}
