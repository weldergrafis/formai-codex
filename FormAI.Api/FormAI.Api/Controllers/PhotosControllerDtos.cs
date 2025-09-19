using System.ComponentModel.DataAnnotations;

namespace FormAI.Api.Controllers;

// Request DTO for scanning directories for photos
public sealed class CreateRequestDto
{
    [Required]
    public long GalleryId { get; set; }

    [Required]
    public string RootPath { get; init; }
}

// Response DTO for scan operation
public sealed class CreateResponseDto
{
    public required int FilesCount { get; set; }
}

// Response DTO for upload operation
public sealed class PhotoUploadResponseDto
{
    public required int UploadedCount { get; set;  }
}
