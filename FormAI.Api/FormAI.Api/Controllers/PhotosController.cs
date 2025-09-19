using FormAI.Api.Data;
using FormAI.Api.Helpers;
using FormAI.Api.Models;
using FormAI.Api.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.IO;

namespace FormAI.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PhotosController(AppDbContext context, StorageService storageService, ServiceBusService serviceBusService) : ControllerBase
{
    [HttpGet("ping")]
    public ActionResult Ping()
    {
        return Ok(DateTime.Now.ToString());
    }

    [HttpPost("create")]
    public async Task<ActionResult<CreateResponseDto>> Create([FromBody] CreateRequestDto request)
    {
        Gallery gallery;

        if (request.GalleryId == 0)
        {
            gallery = new Gallery { Name = request.GalleryId.ToString() };
        }
        else
        {
            gallery = await context.Galleries.SingleAsync(x => x.Id == request.GalleryId);
        }

        // Endpoint to scan directory and register photos
        var files = Directory.EnumerateFiles(request.RootPath, "*.jpg", SearchOption.AllDirectories);


        var photos = files.Select(x => new Photo
        {
            Gallery = gallery,
            LocalPath = x
        });

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

    // Comentário: marca a foto como redimensionada; retorna 404 se não existir e 409 se já estiver marcada
    [HttpPost("{photoId:long}/mark-resized")]
    public async Task<ActionResult> MarkResized(long photoId)
    {
        var photo = await context.Photos.SingleAsync(p => p.Id == photoId);

        if (photo.IsResized) return Conflict(new { message = $"A foto {photoId} já está marcada que foi redimensionada." });

        photo.IsResized = true;
        await context.SaveChangesAsync();

        await serviceBusService.SendMessageAsync($"{photo.Id}", Queue.DetectFaces);

        return NoContent();
    }

    //// Comentário: marca a foto como redimensionada; retorna 404 se não existir e 409 se já estiver marcada
    //[HttpPost("{photoId:long}/mark-faces-detected")]
    //public async Task<ActionResult> MarkFacesDetected(long photoId)
    //{
    //    var photo = await context.Photos.SingleAsync(p => p.Id == photoId);

    //    if (photo.IsFaceDetected) return Conflict(new { message = $"A foto {photoId} já está marcada que suas faces foram detectadas." });

    //    photo.IsFaceDetected = true;
    //    await context.SaveChangesAsync();

    //    return NoContent();
    //}

    public class FaceDto
    {
        public required int NeurotecOrder { get; set; }

        public required byte[] Template { get; set; }

        public required double X { get; set; }
        public required double Y { get; set; }
        public required double Width { get; set; }
        public required double Height { get; set; }

        public required double Pitch { get; set; }
        public required double Roll { get; set; }
        public required double Yaw { get; set; }

        public required NGender Gender { get; set; }
        public required double GenderConfidence { get; set; }

        public required double LeftEyeCenterX { get; set; }
        public required double LeftEyeCenterY { get; set; }
        public required double LeftEyeCenterConfidence { get; set; }
        public required double RightEyeCenterX { get; set; }
        public required double RightEyeCenterY { get; set; }
        public required double RightEyeCenterConfidence { get; set; }
        public required double BothEyesCenterX { get; set; }
        public required double BothEyesCenterY { get; set; }
        public required double BothEyesCenterConfidence { get; set; }
        public required double NoseTipX { get; set; }
        public required double NoseTipY { get; set; }
        public required double NoseTipConfidence { get; set; }
        public required double MouthCenterX { get; set; }
        public required double MouthCenterY { get; set; }
        public required double MouthCenterConfidence { get; set; }

        public required int Quality { get; set; }
        public required double DetectionConfidence { get; set; }
        public required int Occlusion { get; set; }
        public required int Resolution { get; set; }
        public required int MotionBlur { get; set; }
        public required int CompressionArtifacts { get; set; }
        public required int Overexposure { get; set; }
        public required int Underexposure { get; set; }
        public required int GrayscaleDensity { get; set; }
        public required int Sharpness { get; set; }
        public required int Contrast { get; set; }
        public required int BackgroundUniformity { get; set; }
        public required int Saturation { get; set; }
        public required int Noise { get; set; }
        public required int WashedOut { get; set; }
        public required int Pixelation { get; set; }
        public required int Interlace { get; set; }
        public required int Age { get; set; }
        public required int Pose { get; set; }
        public required int EyesOpen { get; set; }
        public required int DarkGlasses { get; set; }
        public required int Glasses { get; set; }
        public required int MouthOpen { get; set; }
        public required int Beard { get; set; }
        public required int Mustache { get; set; }
        public required int HeadCovering { get; set; }
        public required int HeavyFrameGlasses { get; set; }
        public required int LookingAway { get; set; }
        public required int RedEye { get; set; }
        public required int FaceDarkness { get; set; }
        public required int SkinTone { get; set; }
        public required int SkinReflection { get; set; }
        public required int GlassesReflection { get; set; }
        public required int FaceMask { get; set; }
        public required int AdditionalFacesDetected { get; set; }
        public required int GenderMale { get; set; }
        public required int GenderFemale { get; set; }
        public required int Smile { get; set; }
        public required int TokenImageQuality { get; set; }
    }

    public static long LastComparisonOrder = 0;

    [HttpPost("{photoId:long}/create-faces")]
    public async Task<ActionResult> CreateFaces([FromRoute] long photoId, [FromBody] List<FaceDto> request)
    {
        var photo = await context.Photos.SingleAsync(p => p.Id == photoId);

        if (photo.IsFaceDetected) return Conflict(new { message = $"A foto {photoId} já está marcada que suas faces foram detectadas." });

        var faces = request
                 .Select(x => ReflectionHelper.CopyObject<Face>(x))
                 .ToList();

        
        var comparisonOrder = DateTime.Now.Ticks;

        if (comparisonOrder < LastComparisonOrder)
        {
            comparisonOrder = LastComparisonOrder + 1;
        }

        foreach (var face in faces)
        {
            face.PhotoId = photoId;

            // Garante que nenhuma face vai ter a mesma ordem
            comparisonOrder++;
            face.ComparisonOrder = comparisonOrder;
        }

        LastComparisonOrder = comparisonOrder;


        photo.IsFaceDetected = true;

        context.AddRange(faces);

        await context.SaveChangesAsync();


        // Cria as mensagens em paralelo
        await Parallel.ForEachAsync(
           faces,
           //new ParallelOptions { MaxDegreeOfParallelism = 3 },
           async (face, token) =>
           {
               await serviceBusService.SendMessageAsync($"{face.ComparisonOrder}", Queue.CompareFaces);
           });

        return Ok();
    }

}
