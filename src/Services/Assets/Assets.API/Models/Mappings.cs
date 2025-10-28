using Assets.Data.Entities;
using Assets.Data.Repositories;
using System.Threading.Tasks;

namespace Assets.API.Models;

public static class Mappings
{
    public static Asset ToAsset(this CreateAssetRequest request)
                => new()
                {
                    Id = Guid.NewGuid(),
                    Name = request.Name,
                    Type = request.Type,
                    Description = request.Description,
                    Manufacturer = string.IsNullOrEmpty(request.Manufacturer) ? null : request.Manufacturer,
                    ModelNumber = string.IsNullOrEmpty(request.ModelNumber) ? null : request.ModelNumber,
                    SerialNumber = string.IsNullOrEmpty(request.SerialNumber) ? null : request.SerialNumber,
                    CreatedAt = DateTime.UtcNow,
                    LastUpdatedAt = DateTime.UtcNow,
                };

    public static Asset ToAsset(this UpdateAssetRequest request, Guid id)
                => new()
                {
                    Id = Guid.NewGuid(),
                    Name = request.Name,
                    Type = request.Type,
                    Description = request.Description,
                    Manufacturer = string.IsNullOrEmpty(request.Manufacturer) ? null : request.Manufacturer,
                    ModelNumber = string.IsNullOrEmpty(request.ModelNumber) ? null : request.ModelNumber,
                    SerialNumber = string.IsNullOrEmpty(request.SerialNumber) ? null : request.SerialNumber,
                    CreatedAt = DateTime.UtcNow,
                    LastUpdatedAt = DateTime.UtcNow,
                };

    public static AssetResponse ToAssetResponse(this Asset asset)
                => new()
                {
                    Id = asset.Id,
                    Name = asset.Name,
                    Type = asset.Type,
                    Description = asset.Description,
                    Manufacturer = asset.Manufacturer,
                    ModelNumber = asset.ModelNumber,
                    SerialNumber = asset.SerialNumber,
                    CreatedAt = asset.CreatedAt,
                    LastUpdatedAt = asset.LastUpdatedAt,
                };

    public static LocationReference ToLocationReference(this Location location) =>
        new()
        {
            Id = location.Id,
            Name = location.Name
        };

    public static Location ToLocation(this CreateLocationRequest request) =>
        new()
        {
            Id = Guid.NewGuid(),
            Name = request.Name,
            Description = request.Description,
            ParentLocationId = request.ParentLocationId
        };

    public static Location ToLocation(this UpdateLocationRequest req, Guid id) =>
        new()
        {
            Id = id,
            Name = req.Name,
            Description = req.Description,
            LastUpdatedAt = DateTime.Now,
            ParentLocationId = req.ParentLocationId,
        };

    public static LocationResponse ToLocationResponse(this Location location) =>
        new()
        {
            Id = location.Id,
            Name = location.Name,
            Description = location.Description,
            Parent = null,
            Children = [],
            CreatedAt = location.CreatedAt,
            LastUpdatedAt = location.LastUpdatedAt
        };
}