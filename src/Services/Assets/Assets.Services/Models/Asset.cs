using Assets.Data.Entities;

namespace Assets.Services.Models;

public record Asset
{
    public Guid AssetId { get; init; }
    public string? Name { get; init; }
    public string? Description { get; init; }
    public string? Type { get; init; }
    public string? SerialNumber { get; init; }
    public string? ModelNumber { get; init; }
    public string? Manufacturer { get; init; }
    public decimal? PurchaseCost { get; init; }
    public DateTime? PurchaseDate { get; init; }
    public decimal? CurrentValue { get; init; }
    public Guid LocationId { get; init; }
    public Guid OrganisationId { get; init; }
    public DateTime CreatedAt { get; init; }
    public DateTime LastUpdatedAt { get; init; }


    public static Asset FromEntity(Data.Entities.AssetEntity entity) => new()
    {
        AssetId = entity.AssetId,
        Name = entity.Name,
        Description = entity.Description,
        Type = entity.Type,
        SerialNumber = entity.SerialNumber,
        ModelNumber = entity.ModelNumber,
        Manufacturer = entity.Manufacturer,
        PurchaseCost = entity.PurchaseCost,
        PurchaseDate = entity.PurchaseDate,
        CurrentValue = entity.CurrentValue,
        LocationId = entity.LocationId,
        OrganisationId = entity.OrganisationId,
        CreatedAt = entity.CreatedAt,
        LastUpdatedAt = entity.LastUpdatedAt
    };

    public AssetEntity ToEntity() => new()
    {
        AssetId = this.AssetId,
        Name = this.Name,
        Description = this.Description,
        Type = this.Type,
        SerialNumber = this.SerialNumber,
        ModelNumber = this.ModelNumber,
        Manufacturer = this.Manufacturer,
        PurchaseCost = this.PurchaseCost,
        PurchaseDate = this.PurchaseDate,
        CurrentValue = this.CurrentValue,
        LocationId = this.LocationId,
        OrganisationId = this.OrganisationId,
        CreatedAt = this.CreatedAt,
        LastUpdatedAt = this.LastUpdatedAt
    };
}
