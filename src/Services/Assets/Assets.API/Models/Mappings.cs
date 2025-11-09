using Assets.Services.Models;

namespace Assets.API.Models;

public static class Mappings
{
    public static Asset ToAsset(this CreateAssetRequest request)
                => new()
                {
                    AssetId = Guid.NewGuid(),
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
                    AssetId = Guid.NewGuid(),
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
                    Id = awl.AssetId,
                    Name = awl.Name,
                    Type = awl.Type,
                    Description = awl.Description,
                    Manufacturer = awl.Manufacturer,
                    ModelNumber = awl.ModelNumber,
                    SerialNumber = awl.SerialNumber,
                    CreatedAt = awl.CreatedAt,
                    LastUpdatedAt = awl.LastUpdatedAt,
                    Location = new(awl.LocationId, awl.Location?.Name ?? "Unknown")
                };

    public static Location ToLocation(this CreateLocationRequest request) => new()
    {
        LocationId = Guid.NewGuid(),
        Name = request.Name,
        Description = request.Description,
        ParentLocationId = request.ParentLocationId
    };

    public static Location ToLocation(this UpdateLocationRequest req, Guid id) =>
        new()
        {
            LocationId = id,
            Name = req.Name,
            Description = req.Description,
            LastUpdatedAt = DateTime.Now,
            ParentLocationId = req.ParentLocationId,
        };

    public static LocationResponse ToLocationResponse(this Location location) =>
        new()
        {
            Id = location.LocationId,
            Name = location.Name,
            Description = location.Description,
            Parent = null,
            Children = [],
            CreatedAt = location.CreatedAt,
            LastUpdatedAt = location.LastUpdatedAt
        };
}