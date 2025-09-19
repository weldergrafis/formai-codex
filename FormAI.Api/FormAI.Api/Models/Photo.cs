using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FormAI.Api.Models;

public class Photo : ModelBase
{
    [Required]
    public Gallery Gallery { get; set; }

    [ForeignKey(nameof(Gallery))]
    public long GalleryId { get; set; }

    public required string LocalPath { get; set; }

    public bool IsUploaded { get; set; }

    public bool IsResized { get; set; }

    public bool IsFaceDetected { get; set; }

    public IList<Face> Faces { get; set; }
}
