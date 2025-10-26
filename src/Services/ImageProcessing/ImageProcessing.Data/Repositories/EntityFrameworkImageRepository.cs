using ImageProcessing.Data.Context;
using ImageProcessing.Data.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace ImageProcessing.Data.Repositories;

public class EntityFrameworkImageRepository(ImagesDbContext context) : IImageRepository
{
    public async Task AddImageAsync(Image image)
    {
        if (image.Id is null)
            throw new InvalidDataException("Image ID cannot be null.");

        image.CreatedAt = DateTime.UtcNow;
        image.LastUpdatedAt = DateTime.UtcNow;

        context.Images.Add(image);
        await context.SaveChangesAsync().ConfigureAwait(false);

    }

    public async Task DeleteImageAsync(Guid id)
    {
        var image = await context.Images.FindAsync(id).ConfigureAwait(false);
        if (image is not null)
        {
            context.Images.Remove(image);
            await context.SaveChangesAsync().ConfigureAwait(false);
        }
    }

    public async Task<Image?> GetImageByIdAsync(Guid id)
        => await context.Images.FindAsync(id).ConfigureAwait(false);

    public async Task<IEnumerable<Image>> GetImagesByAssetIdAsync(params Guid[] assetIds)
        => await context.Images.Where(img => assetIds.Contains(img.AssetId))
            .ToListAsync()
            .ConfigureAwait(false);

    public async Task<Image?> UpdateImageAsync(Image image)
    {
        if (image.Id is null)
            throw new InvalidDataException("Image ID cannot be null.");

        image.LastUpdatedAt = DateTime.UtcNow;

        context.Images.Update(image);
        await context.SaveChangesAsync().ConfigureAwait(false);
        return image;
    }
}
