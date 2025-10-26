using System;

namespace Assets.Data.Entities;

public class Asset
{
    public Guid Id { get; set; }
    public string? Name { get; set; }
    public string? Description { get; set; }
    public string? Type { get; set; }
    public string? SerialNumber { get; set; }
    public string? ModelNumber { get; set; }
    public string? Manufacturer { get; set; }

    public decimal? PurchaseCost { get; set; }
    public DateTime? PurchaseDate { get; set; }
    public decimal? CurrentValue { get; set; }


    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public DateTime LastUpdatedAt { get; set; } = DateTime.Now;
}
