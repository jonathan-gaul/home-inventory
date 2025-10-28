namespace Assets.API.Models;

public record CreateLocationRequest
{
    public required string Name { get; init; }
    public string? Description { get; set; }
    public Guid? ParentLocationId { get; set; }
    public List<Guid>? Children { get; set; }
}
