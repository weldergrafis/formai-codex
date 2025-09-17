namespace FormAI.Api.Models;

public class Photo : ModelBase
{
    public required string LocalPath { get; set; }

    public bool IsUploaded { get; set; }

    public bool IsResized { get; set; }

    public bool IsFaceDetected { get; set; }
}
