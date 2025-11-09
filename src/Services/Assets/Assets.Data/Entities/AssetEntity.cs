using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace Assets.Data.Entities;

public class AssetEntity
{
    [Key]
    public Guid AssetId { get; set; }

    [MaxLength(200)]
    public string? Name { get; set; }

    [MaxLength(1000)]
    public string? Description { get; set; }

    [MaxLength(100)]
    public string? Type { get; set; }

    [MaxLength(100)]
    public string? SerialNumber { get; set; }

    [MaxLength(100)]
    public string? ModelNumber { get; set; }

    [MaxLength(200)]
    public string? Manufacturer { get; set; }

    [Precision(18, 2)]
    public decimal? PurchaseCost { get; set; }

    public DateTime? PurchaseDate { get; set; }

    [Precision(18, 2)]
    public decimal? CurrentValue { get; set; }

    public Guid LocationId { get; set; }

    public LocationEntity? Location { get; set; } // Navigation property may not be loaded

    public Guid OrganisationId { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public DateTime LastUpdatedAt { get; set; } = DateTime.UtcNow;
}
