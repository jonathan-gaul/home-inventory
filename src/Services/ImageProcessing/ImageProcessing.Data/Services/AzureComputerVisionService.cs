using Azure;
using Azure.AI.Vision.ImageAnalysis;

namespace ImageProcessing.Data.Services;

public class AzureComputerVisionService : IComputerVisionService
{
    private readonly ImageAnalysisClient _client;

    public AzureComputerVisionService(string endpoint, string apiKey)
    {
        var credential = new AzureKeyCredential(apiKey);
        var serviceEndpoint = new Uri(endpoint);
        _client = new ImageAnalysisClient(serviceEndpoint, credential);
    }

    public async Task<ImageAnalysisResult> AnalyzeImageAsync(Stream imageStream)
    {
        var visualFeatures = VisualFeatures.Tags
                     | VisualFeatures.Objects
                     | VisualFeatures.Read;

        var analysisOptions = new ImageAnalysisOptions
        {            
            Language = "en"
        };

        // Convert stream to BinaryData
        var imageData = BinaryData.FromStream(imageStream);

        var result = await _client.AnalyzeAsync(imageData, visualFeatures, analysisOptions).ConfigureAwait(false);

        var ocrText = (result.Value.Read?.Blocks?.Count ?? 0) > 0 ? string.Join('\n', result.Value.Read?.Blocks[0].Lines.Select(x => x.Text) ?? []) : "";

        return new ImageAnalysisResult
        {
            Tags = [.. result.Value.Tags.Values.Select(t => $"{t.Name} ({t.Confidence})")],

            Objects = [.. result.Value.Objects.Values
                .Select(o => new DetectedObject
                {
                    Name = o.Tags.FirstOrDefault()?.Name ?? "unknown",
                    Confidence = o.Tags.FirstOrDefault()?.Confidence ?? 0
                })],

            OcrText = ocrText,

            Caption = result.Value.Caption?.Text
        };
    }
}
