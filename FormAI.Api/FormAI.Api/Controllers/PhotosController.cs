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


    [HttpPost("{photoId}/MarkResized")]
    public async Task<ActionResult<PhotoUploadResponseDto>> MarkResized()
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
        foreach (var photo in photos)
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
}
