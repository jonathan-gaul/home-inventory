namespace Assets.API.Models;

public record LocationResponse
{
    public required Guid Id { get; init; }

    public required string Name { get; init; }
    public string? Description { get; set; }

    public required DateTime CreatedAt { get; init; }
    public required DateTime LastUpdatedAt { get; init; }

    public LocationReference? Parent { get; set; }
    public required List<LocationReference> Children { get; set; } = [];
}
