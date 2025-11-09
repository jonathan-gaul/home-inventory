
namespace Assets.Data.Repositories;

using Assets.Data.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

public interface IAssetRepository
{
    Task<AssetEntity?> GetByIdAsync(Guid id);
    Task<AssetEntity?> GetByIdWithLocationAsync(Guid id);

    Task<IEnumerable<AssetEntity>> GetByLocationIdAsync(Guid id);

    Task<AssetEntity> AddAsync(AssetEntity asset);
    Task<IEnumerable<AssetEntity>> AddManyAsync(params AssetEntity[] asset);
    Task<IEnumerable<AssetEntity>> AddManyWithLocationAsync(params AssetEntity[] assets);

    Task<AssetEntity?> UpdateAsync(AssetEntity asset);
    Task<IEnumerable<AssetEntity>> UpdateManyAsync(params AssetEntity[] assets);
    Task<IEnumerable<AssetEntity>> UpdateManyWithLocationAsync(params AssetEntity[] assets);

    Task DeleteAsync(Guid id);
    Task DeleteManyAsync(params Guid[] ids);
}
