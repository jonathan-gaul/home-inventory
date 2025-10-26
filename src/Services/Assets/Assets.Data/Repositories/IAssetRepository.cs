
namespace Assets.Data.Repositories;

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Assets.Data.Entities;

public interface IAssetRepository
{
    Task<Asset?> GetAssetByIdAsync(Guid id);
    Task<IEnumerable<Asset>> GetAllAssetsAsync();
    Task AddAssetAsync(Asset asset);
    Task<Asset?> UpdateAssetAsync(Asset asset);
    Task DeleteAssetAsync(Guid id);
}
