using System.IO;
using Azure.Storage.Blobs;
using FormAI.Api.Models;
using Microsoft.Extensions.Configuration;

namespace FormAI.Api.Services;

// Service for interacting with Azure Blob Storage
public class StorageService
{
    const string STORAGE_CONNECTION_STRING = "DefaultEndpointsProtocol=https;AccountName=formaist;AccountKey=tA1o3GUJaAtOzgvXvgn3y8BXFmMLhUHWvdxsk+hlHDPL8RvH6h/yd/fjCaDgIeSpa0HDxQJH0oDG+AStFpOFxw==;EndpointSuffix=core.windows.net";
    const string STORAGE_CONTAINER_NAME = "photos";

    private readonly BlobContainerClient _client;

    public StorageService(IConfiguration configuration)
    {
        // Container client is initialized once using the provided connection string
        //var connectionString = configuration.GetConnectionString("AzureStorage") ?? throw new InvalidOperationException("Storage connection string not configured");
        var blobServiceClient = new BlobServiceClient(STORAGE_CONNECTION_STRING);
        _client = blobServiceClient.GetBlobContainerClient(STORAGE_CONTAINER_NAME);
        _client.CreateIfNotExists();
    }

    // Uploads a file to the Originals container and returns the blob URL
    public async Task UploadPhotoAsync(Photo photo, CancellationToken cancellationToken = default)
    {
        var folder = "originals";
        var blobName = $"{folder}/{photo.Id}.jpg";
        var blobClient = _client.GetBlobClient(blobName);
        await using var stream = File.OpenRead(photo.LocalPath);
        await blobClient.UploadAsync(stream, overwrite: true, cancellationToken);
    }

}
