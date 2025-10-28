
namespace Assets.Data.Repositories;

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Assets.Data.Entities;

public interface IAssetRepository
{
    Task<Asset?> GetByIdAsync(Guid id);
    Task<IEnumerable<Asset>> GetAllAssetsAsync();
    Task<IEnumerable<Asset>> GetByLocationIdAsync(Guid id);

    Task<Asset> AddAsync(Asset asset);
    Task<IEnumerable<Asset>> AddManyAsync(params Asset[] asset);
    
    Task<Asset?> UpdateAsync(Asset asset);
    Task<IEnumerable<Asset>> UpdateManyAsync(params Asset[] assets);
    
    Task DeleteAsync(Guid id);
    Task DeleteManyAsync(params Guid[] ids);
}
