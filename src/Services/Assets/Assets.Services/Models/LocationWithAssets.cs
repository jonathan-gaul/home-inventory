using Assets.Data.Entities;

namespace Assets.Services.Models;

public record LocationWithAssets : Location
{
    public IEnumerable<Asset> Assets { get; init; } = [];

    public static new LocationWithAssets FromEntity(LocationEntity entity) => new()
    {
        LocationId = entity.LocationId,
        Name = entity.Name,
        OrganisationId = entity.OrganisationId,
        CreatedAt = entity.CreatedAt,
        LastUpdatedAt = entity.LastUpdatedAt,
        Assets = entity.Assets?.Select(Asset.FromEntity) ?? []
    };
}
