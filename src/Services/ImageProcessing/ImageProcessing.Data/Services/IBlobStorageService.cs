using System;
using System.Collections.Generic;
using System.Net.Mime;
using System.Text;

namespace ImageProcessing.Data.Services;

public interface IBlobStorageService
{
    Task UploadAsync(Guid blobId, string contentType, Stream content);
    Task<Stream?> DownloadAsync(Guid blobId);
    Task DeleteAsync(Guid blobId);
}
