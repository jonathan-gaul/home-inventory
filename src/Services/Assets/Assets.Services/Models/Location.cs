namespace Assets.Services.Models;

public record Location
{
    public Guid LocationId { get; init; }
    public string Name { get; init; }
    public Guid OrganisationId { get; init; }
    public string? Description { get; init; }
    public Guid? ParentLocationId { get; init; }
    public DateTime CreatedAt { get; init; }
    public DateTime LastUpdatedAt { get; init; }

    public static Location FromEntity(Data.Entities.LocationEntity entity) => new()
    {
        LocationId = entity.LocationId,
        Name = entity.Name,
        Description = entity.Description,
        OrganisationId = entity.OrganisationId,
        ParentLocationId = entity.ParentLocationId,
        CreatedAt = entity.CreatedAt,
        LastUpdatedAt = entity.LastUpdatedAt
    };

    public Data.Entities.LocationEntity ToEntity() => new()
    {
        LocationId = this.LocationId,
        Name = this.Name ?? string.Empty,
        Description = this.Description,
        OrganisationId = this.OrganisationId,
        ParentLocationId = this.ParentLocationId,
        CreatedAt = this.CreatedAt,
        LastUpdatedAt = this.LastUpdatedAt
    };
}

