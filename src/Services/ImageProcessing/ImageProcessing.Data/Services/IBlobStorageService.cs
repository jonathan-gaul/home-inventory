namespace ImageProcessing.Data.Services;

public interface IBlobStorageService
{
    Task UploadAsync(Guid blobId, string contentType, Stream content);
    Task<Stream?> DownloadAsync(Guid blobId);
    Task DeleteAsync(Guid blobId);
}
