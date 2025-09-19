//using FormAI.Api.Models;
//using Microsoft.AspNetCore.Http;
//using Microsoft.AspNetCore.Mvc;
//using System.ComponentModel.DataAnnotations;

//namespace FormAI.Api.Controllers
//{
//    [Route("api/[controller]")]
//    [ApiController]
//    public class FacesController : ControllerBase
//    {
        

//        // Request DTO for scanning directories for photos
//        public sealed class CreateRequestDto
//        {
            
//        }

        
//        public async Task<ActionResult<>> Create([FromBody] CreateRequestDto request)
//        {
//            // Endpoint to scan directory and register photos
//            var files = Directory.EnumerateFiles(request.RootPath, "*.jpg", SearchOption.AllDirectories);

//            var photos = files.Select(x => new Photo { LocalPath = x });
//            context.AddRange(photos);

//            await context.SaveChangesAsync();
//            return Ok(new CreateResponseDto { FilesCount = photos.Count() });
//        }
//    }
//}
