using System.IO;
using Azure.Storage.Blobs;
using Microsoft.Extensions.Configuration;

namespace FormAI.Api.Services;

// Service for interacting with Azure Blob Storage
public class AzureBlobStorageService
{
    private readonly BlobContainerClient _containerClient;

    public AzureBlobStorageService(IConfiguration configuration)
    {
        // Container client is initialized once using the provided connection string
        var connectionString = configuration.GetConnectionString("AzureStorage") ?? throw new InvalidOperationException("Storage connection string not configured");
        var blobServiceClient = new BlobServiceClient(connectionString);
        _containerClient = blobServiceClient.GetBlobContainerClient("originals");
        _containerClient.CreateIfNotExists();
    }

    // Uploads a file to the Originals container and returns the blob URL
    public async Task<string> UploadAsync(string localPath, CancellationToken cancellationToken = default)
    {
        var fileName = Path.GetFileName(localPath);
        var blobClient = _containerClient.GetBlobClient(fileName);
        await using var stream = File.OpenRead(localPath);
        await blobClient.UploadAsync(stream, overwrite: true, cancellationToken);
        return blobClient.Uri.ToString();
    }
}
