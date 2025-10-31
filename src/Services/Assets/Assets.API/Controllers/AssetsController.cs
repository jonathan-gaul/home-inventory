using Assets.API.Models;
using Assets.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Assets.API.Controllers;

[ApiController]
[Route("/api/[controller]")]
[Authorize]
public class AssetsController : ControllerBase
{
    private readonly ILogger<AssetsController> _logger;
    private readonly IAssetService _assetService;
    
    public AssetsController(IAssetService assetService)
    {
        _assetService = assetService;
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var asset = await _assetService.GetByIdAsync(id);

        if (asset is null)
            return NotFound();

        return Ok(asset.ToAssetResponse());
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateAssetRequest request)
    {
        var result = await _assetService.CreateAsync(request.ToAsset());

        return CreatedAtAction(nameof(GetById), new { id = result.Asset.Id }, result.ToAssetResponse());
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateAssetRequest request)
    {
        var asset = request.ToAsset();
        asset.Id = id;

        var updatedAsset = await _assetService.UpdateAsync(request.ToAsset());
        if (updatedAsset is null)
            return NotFound();

        return Ok(updatedAsset.ToAssetResponse());
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        await _assetService.DeleteAsync(id);
        return NoContent();
    }

}
