namespace ImageProcessing.Data.Entities
{
    public class Image
    {
        public Guid? Id { get; set; }
        public Guid AssetId { get; set; }
        public string ContentType { get; set; } = "image/png";

        public ImageAnalysisStatus AnalysisStatus { get; set; } = ImageAnalysisStatus.NotAnalysed;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime LastUpdatedAt { get; set; } = DateTime.UtcNow;
    }
}
