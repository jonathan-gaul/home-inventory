using System.ComponentModel.DataAnnotations;

namespace Assets.Data.Entities;

public class LocationEntity
{
    [Key]
    public Guid LocationId { get; set; }

    [Required]
    [MaxLength(200)]
    public string Name { get; set; } = string.Empty;

    [MaxLength(1000)]
    public string? Description { get; set; }

    public Guid? ParentLocationId { get; set; }
    public LocationEntity? ParentLocation { get; set; }
    public ICollection<LocationEntity> ChildLocations { get; set; } = [];

    public Guid OrganisationId { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime LastUpdatedAt { get; set; } = DateTime.UtcNow;

    public ICollection<AssetEntity> Assets { get; set; } = [];

}
