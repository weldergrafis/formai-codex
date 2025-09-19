using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FormAI.Api.Models;

[Index(nameof(ComparisonOrder), IsUnique = true)]
public class Face : ModelBase
{
    [Required]
    public Photo Photo { get; set; }

    [Required]
    [ForeignKey(nameof(Photo))]
    public long PhotoId { get; set; }


    public Person? Person { get; set; }

    [ForeignKey(nameof(Person))]
    public long? PersonId { get; set; }


    // Comentário: coleções “inversas” separadas para cada papel
    [InverseProperty(nameof(FaceComparison.Face1))]
    public ICollection<FaceComparison> FaceComparisonsAsFace1 { get; set; } = [];

    [InverseProperty(nameof(FaceComparison.Face2))]
    public ICollection<FaceComparison> FaceComparisonsAsFace2 { get; set; } = [];


    public bool IsFaceCompared { get; set; }


    public long ComparisonOrder { get; set; }

    // Neurotec
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

public enum NLExpression
{
    //
    // Summary:
    //     Unspecified expression.
    Unspecified = 0,
    //
    // Summary:
    //     Neutral.
    Neutral = 1,
    //
    // Summary:
    //     Smiling.
    Smile = 2,
    //
    // Summary:
    //     Smiling with jaw open.
    SmileOpenedJaw = 3,
    //
    // Summary:
    //     Eyebrows raised.
    RaisedBrows = 4,
    //
    // Summary:
    //     Eyes looking away.
    EyesAway = 5,
    //
    // Summary:
    //     Squinting.
    Squinting = 6,
    //
    // Summary:
    //     Frowning.
    Frowning = 7,
    //
    // Summary:
    //     Unknown expression.
    Unknown = 0xFFFF
}

public enum NGender
{
    //
    // Summary:
    //     Unspecified gender.
    Unspecified = 0,
    //
    // Summary:
    //     Male.
    Male = 1,
    //
    // Summary:
    //     Female.
    Female = 2,
    //
    // Summary:
    //     Unknown.
    Unknown = 0xFF
}

[Flags]
public enum NLProperties
{
    //
    // Summary:
    //     Face properties not specified.
    NotSpecified = 0x0,
    //
    // Summary:
    //     Face properties specified.
    Specified = 0x1,
    //
    // Summary:
    //     Wearing glasses.
    Glasses = 0x2,
    //
    // Summary:
    //     Mustache.
    Mustache = 0x4,
    //
    // Summary:
    //     Beard.
    Beard = 0x8,
    //
    // Summary:
    //     Teeth are visible.
    TeethVisible = 0x10,
    //
    // Summary:
    //     Eye blink.
    Blink = 0x20,
    //
    // Summary:
    //     Mouth is open.
    MouthOpen = 0x40,
    //
    // Summary:
    //     Patch on left eye.
    LeftEyePatch = 0x80,
    //
    // Summary:
    //     Patch on right eye.
    RightEyePatch = 0x100,
    //
    // Summary:
    //     Patch on both eyes.
    BothEyePatch = 0x200,
    //
    // Summary:
    //     Wearing dark glasses.
    DarkGlasses = 0x400,
    //
    // Summary:
    //     Distorting conditions.
    DistortingCondition = 0x800,
    //
    // Summary:
    //     Wearing a hat.
    Hat = 0x1000000,
    //
    // Summary:
    //     Wearing a scarf.
    Scarf = 0x2000000,
    //
    // Summary:
    //     One ear missing.
    NoEar = 0x4000000,
    //
    // Summary:
    //     Wearing a face mask.
    FaceMask = 0x8000000,
    //
    // Summary:
    //     Wearing glasses with heavy frame.
    HeavyFrame = 0x10000000
}

