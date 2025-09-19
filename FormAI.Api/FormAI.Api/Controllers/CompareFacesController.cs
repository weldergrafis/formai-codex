using FormAI.Api.Data;
using FormAI.Api.Helpers;
using FormAI.Api.Models;
using FormAI.Api.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using static FormAI.Api.Controllers.PhotosController;

namespace FormAI.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CompareFacesController(AppDbContext context) : ControllerBase
    {
        // DTO do retorno
        public sealed class FaceDto
        {
            public required long Id { get; set; }
            public required long ComparisonOrder { get; set; }
            public required byte[] Template { get; set; } = Array.Empty<byte>();
        }

        // DTO do request
        public sealed class FacesTemplatesRequest
        {
            public long? Start { get; set; }
            public long? End { get; set; }
        }

        [HttpPost("get-faces-templates")]
        public async Task<ActionResult<List<FaceDto>>> GetFacesTemplates([FromBody] FacesTemplatesRequest request)
        {
            var query = context.Faces.AsNoTracking();

            if (request.Start != null)
                query = query.Where(f => f.ComparisonOrder >= request.Start);

            if (request.End != null)
                query = query.Where(f => f.ComparisonOrder <= request.End);

            var items = await query
                .Select(x => new FaceDto
                {
                    Id = x.Id,
                    ComparisonOrder = x.ComparisonOrder,
                    Template = x.Template
                })
                .OrderBy(f => f.ComparisonOrder)
                .ToListAsync();

            return Ok(items);
        }

        public class FaceComparisonDto
        {
            public required long FaceId { get; set; }
            public required double Score { get; set; }
        }

        [HttpPost("{faceId:long}/create-faces-comparison")]
        public async Task<ActionResult> CreateFacesComparisons([FromQuery] long faceId, [FromBody] List<FaceComparisonDto> request)
        {
            var face = await context.Faces.SingleAsync(x => x.Id == faceId);

            var facesComparisons = request
                .Select(x => new FaceComparison
                {
                    Face1Id = Math.Min(faceId, x.FaceId),
                    Face2Id = Math.Max(faceId, x.FaceId),
                    Score = x.Score
                });


            context.FaceComparisons.AddRange(facesComparisons);

            face.IsFaceCompared = true;

            await context.SaveChangesAsync();

            return Ok();
        }
    }
}
