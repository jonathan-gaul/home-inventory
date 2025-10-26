using Assets.Data.Entities;

namespace Assets.API.Models;

public static class AssetsModelMappings
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
}