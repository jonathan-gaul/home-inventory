namespace Assets.API.Models;

public record UpdateAssetRequest
{
    public string? Name { get; set; }
    public string? Type { get; set; }
    public string? Description { get; set; }
    public string? Manufacturer { get; set; }
    public string? ModelNumber { get; set; }
    public string? SerialNumber { get; set; }
    public required Guid LocationId { get; init; }
}
