using Assets.Data.Entities;

namespace Assets.Data.Repositories;

public interface ILocationRepository
{
    Task<LocationEntity?> GetByIdAsync(Guid id);
    Task<LocationEntity?> GetByIdWithAssetsAsync(Guid id);
    Task<LocationEntity?> GetByIdWithChildrenAsync(Guid id);

    Task<IEnumerable<LocationEntity>> GetManyByIdAsync(params Guid[] ids);
    Task<IEnumerable<LocationEntity>> GetManyByIdWithAssetsAsync(params Guid[] ids);

    Task<IEnumerable<LocationEntity>> GetByParentIdAsync(Guid id);
    Task<IEnumerable<LocationEntity>> GetByParentIdWithAssetsAsync(Guid id);

    Task<LocationEntity> AddAsync(LocationEntity location);
    Task<IEnumerable<LocationEntity>> AddManyAsync(params LocationEntity[] locations);

    Task<LocationEntity?> UpdateAsync(LocationEntity location);
    Task<IEnumerable<LocationEntity>> UpdateManyAsync(params LocationEntity[] locations);

    Task DeleteAsync(Guid id);
    Task DeleteManyAsync(params Guid[] ids);
}
