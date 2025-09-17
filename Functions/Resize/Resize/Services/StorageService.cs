using Azure.Storage.Blobs;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Resize.Services;

public class StorageService
{
    const string STORAGE_CONNECTION_STRING = "DefaultEndpointsProtocol=https;AccountName=formaist;AccountKey=tA1o3GUJaAtOzgvXvgn3y8BXFmMLhUHWvdxsk+hlHDPL8RvH6h/yd/fjCaDgIeSpa0HDxQJH0oDG+AStFpOFxw==;EndpointSuffix=core.windows.net";
    const string STORAGE_CONTAINER_NAME = "photos";

    readonly BlobContainerClient _client;

    public StorageService(IConfiguration configuration)
    {
        // Container client is initialized once using the provided connection string
        //var connectionString = configuration.GetConnectionString("AzureStorage") ?? throw new InvalidOperationException("Storage connection string not configured");
        var blobServiceClient = new BlobServiceClient(STORAGE_CONNECTION_STRING);
        _client = blobServiceClient.GetBlobContainerClient(STORAGE_CONTAINER_NAME);
        _client.CreateIfNotExists();
    }

    public async Task<Stream> DownloadAsync(string blobName)
    {
        var blobClient = _client.GetBlobClient(blobName);
        var ms = new MemoryStream();
        await blobClient.DownloadToAsync(ms);
        ms.Position = 0;
        return ms;
    }

    // Uploads a file to the Originals container and returns the blob URL
    public async Task UploadAsync(string blobName, Stream stream, CancellationToken cancellationToken = default)
    {
        var blobClient = _client.GetBlobClient(blobName);
        await blobClient.UploadAsync(stream, overwrite: true, cancellationToken);
    }
}
