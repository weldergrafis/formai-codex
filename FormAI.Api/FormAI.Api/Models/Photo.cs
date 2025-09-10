namespace FormAI.Api.Models;

public class Photo : ModelBase
{
    // Full path to the photo on disk
    public required string LocalPath { get; set; }

    // Indicates whether the photo has been uploaded to storage
    public bool Uploaded { get; set; }

    // Reference to the uploaded file in Azure Storage
    public string? StorageUrl { get; set; }
}
