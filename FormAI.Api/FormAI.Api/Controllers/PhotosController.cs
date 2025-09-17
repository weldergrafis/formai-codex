using System.IO;

using FormAI.Api.Data;
using FormAI.Api.Models;
using FormAI.Api.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FormAI.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PhotosController(AppDbContext context, StorageService storageService, ServiceBusService serviceBusService) : ControllerBase
{
    [HttpPost("create")]
    public async Task<ActionResult<CreateResponseDto>> Create([FromBody] CreateRequestDto request)
    {
        // Endpoint to scan directory and register photos
        var files = Directory.EnumerateFiles(request.RootPath, "*.jpg", SearchOption.AllDirectories);

        var photos = files.Select(x => new Photo { LocalPath = x });
        context.AddRange(photos);

        await context.SaveChangesAsync();
        return Ok(new CreateResponseDto { FilesCount = photos.Count() });
    }

    [HttpPost("upload")]
    public async Task<ActionResult<PhotoUploadResponseDto>> Upload()
    {
        var photos = await context.Photos.Where(x => !x.IsUploaded).ToListAsync();

        // Envia as fotos em paralelo
        await Parallel.ForEachAsync(
            photos,
            new ParallelOptions { MaxDegreeOfParallelism = 3 },
            async (photo, token) =>
            {
                await storageService.UploadPhotoAsync(photo, token);
                Console.WriteLine($"Foto {photo.Id} enviada");
            });

        // Marca que foram enviadsa
        foreach(var photo in photos)
        {
            photo.IsUploaded = true;
        }

        await context.SaveChangesAsync();

        // Cria as mensagens em paralelo
        await Parallel.ForEachAsync(
           photos,
           //new ParallelOptions { MaxDegreeOfParallelism = 3 },
           async (photo, token) =>
           {
               await serviceBusService.SendMessageAsync($"{photo.Id}", Queue.Resize);
           });

        return Ok(new PhotoUploadResponseDto { UploadedCount = photos.Count });
    }

    // Comentário: marca a foto como redimensionada; retorna 404 se não existir e 409 se já estiver marcada
    [HttpPost("{photoId:long}/mark-resized")]
    public async Task<ActionResult> MarkResized(long photoId)
    {
        var photo = await context.Photos.SingleOrDefaultAsync(p => p.Id == photoId);

        if (photo is null) return NotFound(new { message = $"Foto {photoId} não encontrada." });
        if (photo.IsResized) return Conflict(new { message = $"A foto {photoId} já está marcada que foi redimensionada." });

        photo.IsResized = true;
        await context.SaveChangesAsync();

        return NoContent();
    }

}
