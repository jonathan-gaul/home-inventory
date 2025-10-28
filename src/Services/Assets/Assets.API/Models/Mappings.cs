using Assets.Data.Entities;
using Assets.Data.Repositories;
using Assets.Services.Models;
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

    public static Asset ToAsset(this UpdateAssetRequest request)
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

    public static AssetResponse ToAssetResponse(this AssetWithLocation awl)
                => new()
                {
                    Id = awl.Asset.Id,
                    Name = awl.Asset.Name,
                    Type = awl.Asset.Type,
                    Description = awl.Asset.Description,
                    Manufacturer = awl.Asset.Manufacturer,
                    ModelNumber = awl.Asset.ModelNumber,
                    SerialNumber = awl.Asset.SerialNumber,
                    CreatedAt = awl.Asset.CreatedAt,
                    LastUpdatedAt = awl.Asset.LastUpdatedAt,
                    Location = new(awl.Asset.LocationId, awl.Location?.Name ?? "Unknown")
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