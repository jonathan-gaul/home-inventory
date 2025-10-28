using Assets.Data.Context;
using Assets.Data.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Assets.Data.Repositories;

public class EntityFrameworkAssetRepository(AssetsDbContext context) : IAssetRepository
{
    public async Task<Asset> AddAsync(Asset asset)
        => (await AddManyAsync(asset)).First();

    public async Task<IEnumerable<Asset>> AddManyAsync(params Asset[] assets)
    {
        foreach (var asset in assets)
        {
            asset.Id = Guid.NewGuid();
            asset.CreatedAt = DateTime.UtcNow;
            asset.LastUpdatedAt = DateTime.UtcNow;

            context.Assets.Add(asset);
        }

        await context.SaveChangesAsync();

        return assets;
    }

    public async Task<Asset?> GetByIdAsync(Guid id)
        => await context.Assets.FindAsync(id);

    public async Task<IEnumerable<Asset>> GetByLocationIdAsync(Guid id)
        => await context.Assets.Where(x => x.LocationId == id).ToListAsync();

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

    public async Task<IEnumerable<Asset>> GetAllAssetsAsync()
        => (await context.Assets.ToListAsync()).AsEnumerable();


    public async Task<Asset?> UpdateAsync(Asset asset)
        => (await UpdateManyAsync(asset)).FirstOrDefault();

    public async Task<IEnumerable<Asset>> UpdateManyAsync(params Asset[] assets)
    {
        foreach (var asset in assets)
        {
            asset.LastUpdatedAt = DateTime.UtcNow;
            context.Assets.Update(asset);
        }

        await context.SaveChangesAsync();

        return assets;
    }
}
