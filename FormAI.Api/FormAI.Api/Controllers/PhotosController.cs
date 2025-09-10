using System.IO;
using FormAI.Api.Controllers; // DTOs
using FormAI.Api.Data;
using FormAI.Api.Models;
using FormAI.Api.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FormAI.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PhotosController : ControllerBase
{
    private readonly AppDbContext _context;
    private readonly AzureBlobStorageService _storageService;

    public PhotosController(AppDbContext context, AzureBlobStorageService storageService)
    {
        _context = context;
        _storageService = storageService;
    }

    [HttpPost("scan")]
    public async Task<ActionResult<PhotoScanResponseDto>> Scan([FromBody] PhotoScanRequestDto request)
    {
        // Endpoint to scan directory and register photos
        var files = Directory.EnumerateFiles(request.RootPath, "*.jpg", SearchOption.AllDirectories);
        var added = 0;
        foreach (var file in files)
        {
            if (!await _context.Photos.AnyAsync(p => p.LocalPath == file))
            {
                _context.Photos.Add(new Photo { LocalPath = file });
                added++;
            }
        }
        await _context.SaveChangesAsync();
        return Ok(new PhotoScanResponseDto(added));
    }

    [HttpPost("upload")]
    public async Task<ActionResult<PhotoUploadResponseDto>> Upload([FromBody] PhotoUploadRequestDto request)
    {
        // Endpoint to upload pending photos to Azure Storage
        var photos = await _context.Photos.Where(p => !p.Uploaded).ToListAsync();
        var uploaded = 0;
        foreach (var photo in photos)
        {
            var storageUrl = await _storageService.UploadAsync(photo.LocalPath);
            photo.StorageUrl = storageUrl;
            photo.Uploaded = true;
            uploaded++;
        }
        await _context.SaveChangesAsync();
        return Ok(new PhotoUploadResponseDto(uploaded));
    }
}
