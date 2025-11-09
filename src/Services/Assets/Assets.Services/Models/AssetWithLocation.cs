namespace Assets.Services.Models;

public record AssetWithLocation : Asset
{
    public Location Location { get; init; }

    public static new AssetWithLocation FromEntity(Data.Entities.AssetEntity entity) => new()
    {
        AssetId = entity.AssetId,
        Name = entity.Name,
        ModelNumber = entity.ModelNumber,
        CurrentValue = entity.CurrentValue,
        Manufacturer = entity.Manufacturer,
        PurchaseCost = entity.PurchaseCost,
        PurchaseDate = entity.PurchaseDate,
        Description = entity.Description,
        SerialNumber = entity.SerialNumber,        
        OrganisationId = entity.OrganisationId,
        CreatedAt = entity.CreatedAt,
        LastUpdatedAt = entity.LastUpdatedAt,
        LocationId = entity.Location is not null ? entity.Location.LocationId : Guid.Empty,
        Location = entity.Location is not null ? Location.FromEntity(entity.Location) : new Location { LocationId = Guid.Empty, Name = "Unknown" }
    };
}
