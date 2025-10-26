namespace ImageProcessing.API.Models;

public class ImageResponse
{
    public Guid Id { get; set; }
    public Guid AssetId { get; set; }
    public string ContentType { get; set; }
    public string AnalysisStatus { get; set; }

    public DateTime CreatedAt { get; set; }
}

