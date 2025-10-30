using FluentValidation;

namespace Assets.API.Models;

public record AssetRequest
{
    public string? Name { get; set; }
    public string? Description { get; set; }
    public string? Type { get; set; }
    public string? Manufacturer {  get; set; }
    public string? ModelNumber { get; set; }
    public string? SerialNumber { get; set; }
    public required Guid LocationId { get; init; }
}

public class AssetRequestValidator : AbstractValidator<AssetRequest>
{
    public AssetRequestValidator()
    {
        RuleFor(x => x.Name)
            .MaximumLength(200);

        RuleFor(x => x.Description)
            .MaximumLength(1000);

        RuleFor(x => x.Type)
            .MaximumLength(100);

        RuleFor(x => x.Manufacturer)
            .MaximumLength(100);

        RuleFor(x => x.ModelNumber)
            .MaximumLength(100);

        RuleFor(x => x.SerialNumber)
            .MaximumLength(100);

        RuleFor(x => x.LocationId)
            .NotEmpty();
    }
}

