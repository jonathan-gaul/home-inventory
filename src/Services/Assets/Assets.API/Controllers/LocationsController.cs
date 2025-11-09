using Assets.API.Models;
using Assets.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Assets.API.Controllers;

[ApiController]
[Route("/api/[controller]")]
[Authorize]
public class LocationsController : ControllerBase
{
    private readonly ILogger<LocationsController> _logger;
    private readonly ILocationService _locationService;

    public LocationsController(ILocationService locationService)
    {
        _locationService = locationService;
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var location = await _locationService.GetByIdAsync(id);
        if (location is null)
            return NotFound();

        var parent = location.ParentLocationId is not null ? await _locationService.GetByIdAsync(location.ParentLocationId.Value) : null;
        var children = await _locationService.GetByParentIdAsync(id);

        return Ok(location.ToLocationResponse() with
        {
            Parent = location.ParentLocationId is not null ? new(location.ParentLocationId.Value, parent?.Name ?? "Unknown") : null,
            Children = [.. children.Select(x => new LocationReference(x.LocationId, x.Name))]
        });
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateLocationRequest request)
    {
        var result = await _locationService.CreateAsync(request.ToLocation());
        if (result is null)
            return NotFound();

        return CreatedAtAction(nameof(GetById), new { id = result.LocationId }, result.ToLocationResponse());
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateLocationRequest request)
    {
        var updatedLocation = await _locationService.UpdateAsync(request.ToLocation(id));

        if (updatedLocation is null)
            return NotFound();

        var parentLocation = request.ParentLocationId is not null
            ? await _locationService.GetByIdAsync(request.ParentLocationId.Value)
            : null;

        if (request.Children is not null)
            await _locationService.ReparentChildrenAsync(updatedLocation.LocationId, request.ParentLocationId, [.. request.Children]);

        var children = await _locationService.GetByParentIdAsync(id);

        var response = updatedLocation is null ? null : updatedLocation.ToLocationResponse() with
        {
            Parent = parentLocation is not null ? new(parentLocation.LocationId, parentLocation.Name) : null,
            Children = [.. children.Select(x => new LocationReference(x.LocationId, x.Name))]
        };

        return Ok(response);
    }
}
