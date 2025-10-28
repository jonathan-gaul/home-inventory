using Assets.Data.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Assets.Services.Models;

public record LocationWithAssets(Location? Location, IEnumerable<Asset> Assets);
