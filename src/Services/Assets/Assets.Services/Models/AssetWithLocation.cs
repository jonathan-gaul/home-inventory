using Assets.Data.Entities;

namespace Assets.Services.Models;

public record AssetWithLocation(Asset Asset, Location? Location);
