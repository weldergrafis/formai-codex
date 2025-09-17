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

        await Parallel.ForEachAsync(
            photos,
            new ParallelOptions { MaxDegreeOfParallelism = 3 },
            async (photo, token) =>
            {
                // Comentário: upload da foto em paralelo
                await storageService.UploadPhotoAsync(photo, token);
                photo.IsUploaded = true;

                Console.WriteLine($"Foto {photo.Id} enviada");
            });


        await context.SaveChangesAsync();

        // Cria as mensagens
        await Parallel.ForEachAsync(
           photos,
           //new ParallelOptions { MaxDegreeOfParallelism = 3 },
           async (photo, token) =>
           {
               await serviceBusService.SendMessageAsync($"{photo.Id}", Queue.Resize);
               Console.WriteLine($"Mensagem criada. Foto: {photo.Id} - Fila: {Queue.Resize}");
           });

        return Ok(new PhotoUploadResponseDto { UploadedCount = photos.Count });
    }
}
