namespace Assets.API.Models;

public record LocationReference
{
    public required Guid Id { get; init; }
    public required string Name { get; init; }
}
