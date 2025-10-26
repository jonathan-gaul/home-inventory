﻿using ImageProcessing.Data.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace ImageProcessing.Data.Repositories;

public interface IImageRepository
{
    public Task AddImageAsync(Image image);
    public Task<Image?> GetImageByIdAsync(Guid id);
    public Task<IEnumerable<Image>> GetImagesByAssetIdAsync(params Guid[] assetIds);
    public Task<Image?> UpdateImageAsync(Image image);
    public Task DeleteImageAsync(Guid id);

}
