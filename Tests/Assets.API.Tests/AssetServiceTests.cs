using Assets.Data.Entities;
using Assets.Data.Repositories;
using Assets.Services;
using Moq;

namespace Assets.API.Tests;

public class AssetServiceTests
{
    private AssetService _assetService;
    private Mock<IAssetRepository> _assetRepositoryMock;
    private Mock<ILocationService> _locationServiceMock;

    [SetUp]
    public void Setup()
    {
        _assetRepositoryMock = new Mock<IAssetRepository>();
        _locationServiceMock = new Mock<ILocationService>();

        _assetService = new AssetService(_assetRepositoryMock.Object, _locationServiceMock.Object);
    }

    [Test]
    public async Task GetByIdAsync_WhenAssetExists_ReturnsAssetWithLocation()
    {
        // Arrange
        var locationId = Guid.NewGuid();
        var expectedLocation = new Location
        {
            Id = locationId,
            Name = "Test Location",
        };

        var assetId = Guid.NewGuid();
        var expectedAsset = new Asset
        {
            Id = assetId,
            Name = "Test",
            LocationId = locationId,
        };

        _locationServiceMock
            .Setup(r => r.GetByIdAsync(locationId))
            .ReturnsAsync(expectedLocation);

        _assetRepositoryMock
            .Setup(r => r.GetByIdAsync(assetId))
            .ReturnsAsync(expectedAsset);

        // Act
        var result = await _assetService.GetByIdAsync(assetId);

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result.Asset.Id, Is.EqualTo(assetId));
        Assert.That(result.Location, Is.Not.Null);
        Assert.That(result.Location.Id, Is.EqualTo(locationId));

        Assert.Pass();
    }
}
