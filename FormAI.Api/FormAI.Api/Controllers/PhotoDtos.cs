namespace FormAI.Api.Controllers;

// Request DTO for scanning directories for photos
public sealed class PhotoScanRequestDto
{
    public required string RootPath { get; init; }
}

// Response DTO for scan operation
public sealed class PhotoScanResponseDto
{
    public PhotoScanResponseDto(int addedCount) => AddedCount = addedCount;
    public int AddedCount { get; }
}

// Request DTO for uploading pending photos
public sealed class PhotoUploadRequestDto
{
}

// Response DTO for upload operation
public sealed class PhotoUploadResponseDto
{
    public PhotoUploadResponseDto(int uploadedCount) => UploadedCount = uploadedCount;
    public int UploadedCount { get; }
}
