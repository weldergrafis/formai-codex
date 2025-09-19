using Neurotec.Biometrics;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using static DetectFaces.Services.NeurotecService;

namespace DetectFaces;

public class FormaiApiClient(HttpClient httpClient)
{

    // Marca a foto como redimensionada
    public async Task MarkFacesDetectedAsync(long photoId)
    {
        var response = await httpClient.PostAsync($"{photoId}/mark-faces-detected", null);
        response.EnsureSuccessStatusCode();
    }


    //public sealed class FaceDto
    //{
    //    public required byte[] Template { get; set; }
    //    public required double X { get; set; }
    //    public required double Y { get; set; }
    //    public required double Width { get; set; }
    //    public required double Height { get; set; }
    //}

   

    [Serializable]
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
        Unknown = 255
    }

    [Serializable]
    [Flags]
    public enum NIcaoWarnings
    {
        None = 0,
        //
        // Summary:
        //     No warnings.
        FaceNotDetected = 1,
        //
        // Summary:
        //     Indicates that face was not detected.
        RollLeft = 2,
        //
        // Summary:
        //     Indicates face roll left.
        RollRight = 4,
        //
        // Summary:
        //     Indicates face roll right.
        YawLeft = 8,
        //
        // Summary:
        //     Indicates face yaw left warning.
        YawRight = 0x10,
        //
        // Summary:
        //     Indicates face yaw right warning.
        PitchUp = 0x20,
        //
        // Summary:
        //     Indicates pitch up.
        PitchDown = 0x40,
        //
        // Summary:
        //     Indicates pitch down.
        TooNear = 0x80,
        //
        // Summary:
        //     Indicates that face is too near.
        TooFar = 0x100,
        //
        // Summary:
        //     Indicates that face is too far.
        TooNorth = 0x200,
        //
        // Summary:
        //     Indicates that face is too north.
        TooSouth = 0x400,
        //
        // Summary:
        //     Indicates that face is too south.
        TooEast = 0x800,
        //
        // Summary:
        //     Indicates that face is too east.
        TooWest = 0x1000,
        //
        // Summary:
        //     Indicates that face is too west.
        Sharpness = 0x2000,
        //
        // Summary:
        //     Indicates sharpness warning.
        BackgroundUniformity = 0x4000,
        //
        // Summary:
        //     Indicates background uniformity.
        GrayscaleDensity = 0x8000,
        //
        // Summary:
        //     Indicates grayscale density.
        Saturation = 0x10000,
        //
        // Summary:
        //     Indicates saturation warning.
        Expression = 0x20000,
        //
        // Summary:
        //     Indicates face expression.
        DarkGlasses = 0x40000,
        //
        // Summary:
        //     Indicates that dark glasses detected.
        Blink = 0x80000,
        //
        // Summary:
        //     Indicates blink of eye.
        MouthOpen = 0x100000,
        //
        // Summary:
        //     Indicates opened mouth.
        LookingAway = 0x200000,
        //
        // Summary:
        //     Indicates subject looking away.
        RedEye = 0x400000,
        //
        // Summary:
        //     Red eye was detected.
        FaceDarkness = 0x800000,
        //
        // Summary:
        //     Indicates that face is too dark.
        UnnaturalSkinTone = 0x1000000,
        //
        // Summary:
        //     Skin tone is unnatural color.
        WashedOut = 0x2000000,
        //
        // Summary:
        //     Face is washed out.
        Pixelation = 0x4000000,
        //
        // Summary:
        //     Pixelation of an image.
        SkinReflection = 0x8000000,
        //
        // Summary:
        //     Light reflection on skin was detected.
        GlassesReflection = 0x10000000,
        //
        // Summary:
        //     Indicates reflection on glasses.
        HeavyFrame = 0x20000000,
        //
        // Summary:
        //     Indicates that glasses have a thick frame.
        Liveness = 0x40000000
    }

    public class FaceDto
    {
        public int NeurotecOrder { get; set; }

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

    public static FaceDto ConvertNeurotecFaceToDto(NeurotecFace neurotecFace, double imageWidth, double imageHeight)
    {
        var nlAtributtes = neurotecFace.NLAtributtes;

        int GetValue(NBiometricAttributeId attributeId)
        {
            return nlAtributtes.GetAttributeValue(attributeId);
        }

        var faceDTO = new FaceDto
        {
            Template = neurotecFace.Template,

            X = nlAtributtes.BoundingRect.X / imageWidth,
            Y = nlAtributtes.BoundingRect.Y / imageHeight,
            Width = nlAtributtes.BoundingRect.Width / imageWidth,
            Height = nlAtributtes.BoundingRect.Height / imageHeight,

            Yaw = nlAtributtes.Yaw,
            Roll = nlAtributtes.Roll,
            Pitch = nlAtributtes.Pitch,

            Gender = (NGender)nlAtributtes.Gender,
            GenderConfidence = nlAtributtes.GenderConfidence,

            LeftEyeCenterX = nlAtributtes.LeftEyeCenter.X / imageWidth,
            LeftEyeCenterY = nlAtributtes.LeftEyeCenter.Y / imageHeight,
            LeftEyeCenterConfidence = nlAtributtes.LeftEyeCenter.Confidence,

            RightEyeCenterX = nlAtributtes.RightEyeCenter.X / imageWidth,
            RightEyeCenterY = nlAtributtes.RightEyeCenter.Y / imageHeight,
            RightEyeCenterConfidence = nlAtributtes.RightEyeCenter.Confidence,

            BothEyesCenterX = nlAtributtes.BothEyesCenter.X / imageWidth,
            BothEyesCenterY = nlAtributtes.BothEyesCenter.Y / imageHeight,
            BothEyesCenterConfidence = nlAtributtes.BothEyesCenter.Confidence,

            NoseTipX = nlAtributtes.NoseTip.X / imageWidth,
            NoseTipY = nlAtributtes.NoseTip.Y / imageHeight,
            NoseTipConfidence = nlAtributtes.NoseTip.Confidence,

            MouthCenterX = nlAtributtes.MouthCenter.X / imageWidth,
            MouthCenterY = nlAtributtes.MouthCenter.Y / imageHeight,
            MouthCenterConfidence = nlAtributtes.MouthCenter.Confidence,


            // NLAtributtes
            Quality = GetValue(NBiometricAttributeId.Quality),
            DetectionConfidence = GetValue(NBiometricAttributeId.DetectionConfidence),
            Occlusion = GetValue(NBiometricAttributeId.Occlusion),
            Resolution = GetValue(NBiometricAttributeId.Resolution),
            MotionBlur = GetValue(NBiometricAttributeId.MotionBlur),
            CompressionArtifacts = GetValue(NBiometricAttributeId.CompressionArtifacts),
            Overexposure = GetValue(NBiometricAttributeId.Overexposure),
            Underexposure = GetValue(NBiometricAttributeId.Underexposure),
            GrayscaleDensity = GetValue(NBiometricAttributeId.GrayscaleDensity),
            Sharpness = GetValue(NBiometricAttributeId.Sharpness),
            Contrast = GetValue(NBiometricAttributeId.Contrast),
            BackgroundUniformity = GetValue(NBiometricAttributeId.BackgroundUniformity),
            Saturation = GetValue(NBiometricAttributeId.Saturation),
            Noise = GetValue(NBiometricAttributeId.Noise),
            WashedOut = GetValue(NBiometricAttributeId.WashedOut),
            Pixelation = GetValue(NBiometricAttributeId.Pixelation),
            Interlace = GetValue(NBiometricAttributeId.Interlace),

            Age = GetValue(NBiometricAttributeId.Age),
            Pose = GetValue(NBiometricAttributeId.Pose),
            EyesOpen = GetValue(NBiometricAttributeId.EyesOpen),
            DarkGlasses = GetValue(NBiometricAttributeId.DarkGlasses),
            Glasses = GetValue(NBiometricAttributeId.Glasses),
            MouthOpen = GetValue(NBiometricAttributeId.MouthOpen),
            Beard = GetValue(NBiometricAttributeId.Beard),
            Mustache = GetValue(NBiometricAttributeId.Mustache),
            HeadCovering = GetValue(NBiometricAttributeId.HeadCovering),
            HeavyFrameGlasses = GetValue(NBiometricAttributeId.HeavyFrameGlasses),
            LookingAway = GetValue(NBiometricAttributeId.LookingAway),
            RedEye = GetValue(NBiometricAttributeId.RedEye),
            FaceDarkness = GetValue(NBiometricAttributeId.FaceDarkness),
            SkinTone = GetValue(NBiometricAttributeId.SkinTone),
            SkinReflection = GetValue(NBiometricAttributeId.SkinReflection),
            GlassesReflection = GetValue(NBiometricAttributeId.GlassesReflection),
            FaceMask = GetValue(NBiometricAttributeId.FaceMask),
            AdditionalFacesDetected = GetValue(NBiometricAttributeId.AdditionalFacesDetected),
            GenderMale = GetValue(NBiometricAttributeId.GenderMale),
            GenderFemale = GetValue(NBiometricAttributeId.GenderFemale),
            Smile = GetValue(NBiometricAttributeId.Smile),
            TokenImageQuality = GetValue(NBiometricAttributeId.TokenImageQuality),
        };

        return faceDTO;


    }

    // Marca a foto como redimensionada
    public async Task CreateFaces(long photoId, List<FaceDto> faces)
    {
        var json = JsonSerializer.Serialize(faces);
        var content = new StringContent(json, Encoding.UTF8, "application/json");
        var response = await httpClient.PostAsync($"{photoId}/create-faces", content);
        response.EnsureSuccessStatusCode();
    }

}
