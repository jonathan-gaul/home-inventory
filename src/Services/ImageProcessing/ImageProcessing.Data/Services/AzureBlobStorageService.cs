using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using System.IO;

namespace ImageProcessing.Data.Services;

public class AzureBlobStorageService : IBlobStorageService
{
    private readonly BlobContainerClient _containerClient;


    public AzureBlobStorageService(BlobServiceClient client, string containerName)
    {
        _containerClient = client.GetBlobContainerClient(containerName);
    }

    public async Task DeleteAsync(Guid blobId)
    {
        var blobClient = _containerClient.GetBlobClient(blobId.ToString());
        await blobClient.DeleteIfExistsAsync().ConfigureAwait(false);
    }
    public Task<Stream?> DownloadAsync(Guid blobId)
    {
        throw new NotImplementedException();
    }
    public async Task UploadAsync(Guid blobId, string contentType, Stream content)
    {
        var blobClient = _containerClient.GetBlobClient(blobId.ToString());
        
        await blobClient.UploadAsync(content, new BlobUploadOptions
        {
            HttpHeaders = new() { ContentType = contentType }
        }).ConfigureAwait(false);
    }
}
