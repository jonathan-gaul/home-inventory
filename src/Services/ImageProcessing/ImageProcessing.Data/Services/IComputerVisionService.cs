namespace ImageProcessing.Data.Services;

public interface IComputerVisionService
{
    Task<ImageAnalysisResult> AnalyzeImageAsync(Stream imageStream);
}

public class ImageAnalysisResult
{
    public List<string> Tags { get; set; } = [];
    public List<DetectedObject> Objects { get; set; } = [];
    public string? OcrText { get; set; }
    public string? Caption { get; set; }
}

public class DetectedObject
{
    public string Name { get; set; } = string.Empty;
    public double Confidence { get; set; }
}