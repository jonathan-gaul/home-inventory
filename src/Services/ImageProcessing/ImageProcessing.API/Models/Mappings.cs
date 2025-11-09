using ImageProcessing.Data.Entities;

namespace ImageProcessing.API.Models;

public static class ImageMappings
{
    public static ImageResponse ToResponse(this Image image)
        => new()
        {
            Id = image.Id!.Value,
            AssetId = image.AssetId,
            CreatedAt = image.CreatedAt,
            ContentType = image.ContentType,
            AnalysisStatus = image.AnalysisStatus switch
            {
                ImageAnalysisStatus.NotAnalysed => "Not Analysed",
                var other => other.ToString(),
            }
        };
}
