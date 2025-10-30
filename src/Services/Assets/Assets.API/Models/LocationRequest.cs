using FluentValidation;

namespace Assets.API.Models;

public record LocationRequest
{
    public required string Name { get; init; }
    public string? Description { get; set; }
    public Guid? ParentLocationId { get; set; }
    public List<Guid>? Children { get; set; }
}

public class LocationRequestValidator : AbstractValidator<LocationRequest>
{
    public LocationRequestValidator()
    {
        RuleFor(x => x.Name)
            .MaximumLength(200);

        RuleFor(x => x.Description)
            .MaximumLength(1000);
    }
}
