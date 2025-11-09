using Assets.Data.Entities;
using Assets.Data.Repositories;
using Assets.Services;
using Assets.Services.Models;
using Moq;

namespace Assets.API.Tests;

public class AssetServiceTests
{
    private AssetService _assetService;
    private Mock<IAssetRepository> _assetRepositoryMock;

    [SetUp]
    public void Setup()
    {
        _assetRepositoryMock = new Mock<IAssetRepository>();

        _assetService = new AssetService(_assetRepositoryMock.Object);
    }

    [Test]
    public async Task GetByIdWithLocationAsync_WhenAssetExists_ReturnsAssetWithLocation()
    {
        // Arrange
        var locationId = Guid.NewGuid();
        var expectedLocation = new Location
        {
            LocationId = locationId,
            Name = "Test Location",
        };

        var assetId = Guid.NewGuid();
        var expectedAsset = new AssetEntity
        {
            AssetId = assetId,
            Name = "Test",
            LocationId = locationId,
            Location = new LocationEntity
            {
                LocationId = locationId,
                Name = "Test Location",
            }
        };

        _assetRepositoryMock
            .Setup(r => r.GetByIdWithLocationAsync(assetId))
            .ReturnsAsync(expectedAsset);

        // Act
        var result = await _assetService.GetByIdAsync(assetId);

        // Assert        
        Assert.Multiple(() =>
        {
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Name, Is.EqualTo("Test"));
            Assert.That(result.AssetId, Is.EqualTo(assetId));
            Assert.That(result.LocationId, Is.EqualTo(locationId));
            Assert.That(result.Location, Is.Not.Null);
            Assert.That(result.Location.LocationId, Is.EqualTo(locationId));
            Assert.That(result.Location.Name, Is.EqualTo("Test Location"));
        });
    }

    [Test]
    public async Task GetByIdWithLocationAsync_WhenAssetDoesNotExist_ReturnsNull()
    {
        // Arrange

        // Act
        var result = await _assetService.GetByIdAsync(Guid.NewGuid());

        // Assert
        Assert.That(result, Is.Null);
    }

    [Test]
    public async Task GetByIdWithLocationAsync_WhenLocationDoesNotExist_ReturnsUnknownLocation()
    {
        // Arrange
        var locationId = Guid.NewGuid();
        var assetId = Guid.NewGuid();
        var expectedAsset = new AssetEntity
        {
            AssetId = assetId,
            Name = "Test",
            LocationId = locationId,
            Location = null
        };
        _assetRepositoryMock
            .Setup(r => r.GetByIdWithLocationAsync(assetId))
            .ReturnsAsync(expectedAsset);

        // Act
        var result = await _assetService.GetByIdAsync(assetId);

        // Assert        
        Assert.Multiple(() =>
        {
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Name, Is.EqualTo("Test"));
            Assert.That(result.AssetId, Is.EqualTo(assetId));
            Assert.That(result.LocationId, Is.EqualTo(Guid.Empty));
            Assert.That(result.Location, Is.Not.Null);
            Assert.That(result.Location.LocationId, Is.EqualTo(Guid.Empty));
            Assert.That(result.Location.Name, Is.EqualTo("Unknown"));
        });
    }
}
