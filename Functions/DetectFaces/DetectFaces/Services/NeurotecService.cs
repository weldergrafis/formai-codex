using Neurotec.Biometrics.Client;
using Neurotec.Biometrics;
using Neurotec.Licensing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Neurotec.Images;
using Neurotec.IO;

namespace DetectFaces.Services
{
    public class NeurotecService(NBiometricClient nBiometricClient) 
    {
        public static NBiometricClient CreateBiometricClient()
        {
            return new NBiometricClient()
            {
                BiometricTypes = NBiometricType.Face,

                //UseDeviceManager = true,
                FacesTemplateSize = NTemplateSize.Large,
                FacesMatchingSpeed = NMatchingSpeed.Low,

                FacesDetectAllFeaturePoints = true,
                FacesDetermineGender = true,
                FacesDetermineAge = true,
                FacesDetectProperties = true,

                FacesMinimalInterOcularDistance = 8,
                FacesCheckIcaoCompliance = true,
            };
        }

        public class DetectFacesResult
        {
            // TODO: O ideal seria pegar as dimensões da foto em outro lugar e não no momento da detecção
            public required System.Drawing.Size ImageSize { get; set; }
            public required List<NeurotecFace> Faces { get; set; }
        }

        public class NeurotecFace
        {
            public NLAttributes NLAtributtes { get; set; }
            public byte[] Template { get; set; }
        }

        public async Task<DetectFacesResult> DetectFacesAsync(Stream stream)
        {
            //using var nBiometricClient = CreateBiometricClient();
            using var nSubject = new NSubject();
            using var nStream = NStream.FromStream(stream);
            using var nImage = NImage.FromStream(nStream);
            using var nFace = new NFace() { Image = nImage };

            nSubject.Faces.Add(nFace);
            nSubject.IsMultipleSubjects = true;


            var nTaskDetect = nBiometricClient.CreateTask(
                NBiometricOperations.Detect |
                NBiometricOperations.DetectSegments |
                NBiometricOperations.CreateTemplate,
                nSubject);


            await nBiometricClient.PerformTaskAsync(nTaskDetect);

            if (nTaskDetect.Status == NBiometricStatus.Ok ||
                nTaskDetect.Status == NBiometricStatus.ObjectNotFound ||
                nTaskDetect.Status == NBiometricStatus.SpoofDetected ||
                nTaskDetect.Status == NBiometricStatus.Occlusion ||
                nTaskDetect.Status == NBiometricStatus.BadSharpness ||
                nTaskDetect.Status == NBiometricStatus.CompressionArtifacts ||
                nTaskDetect.Status == NBiometricStatus.BadLighting ||
                nTaskDetect.Status == NBiometricStatus.BadPose ||
                nTaskDetect.Status == NBiometricStatus.MotionBlur
                )
            {
                var faces = new List<NeurotecFace>();

                for (int i = 0; i < nFace.Objects.Count; i++)
                {
                    var nlAtributes = nFace.Objects[i];

                    NSubject nSubjectRelated;

                    if (i == 0)
                        nSubjectRelated = nSubject;
                    else
                        nSubjectRelated = nSubject.RelatedSubjects[i - 1];

                    using var templateBuffer = nSubjectRelated.GetTemplateBuffer();

                    var neurotecFace = new NeurotecFace()
                    {
                        NLAtributtes = nlAtributes,
                        Template = templateBuffer.ToArray()
                    };

                    if (i != 0)
                        nSubjectRelated.Dispose();

                    faces.Add(neurotecFace);
                }

                var result = new DetectFacesResult()
                {
                    Faces = faces,
                    ImageSize = new System.Drawing.Size((int)nImage.Width, (int)nImage.Height)
                };

                return (result);
            }
            else
            {
                throw new Exception($"Erro ao detectar as faces. Status: {nTaskDetect.Status}", nTaskDetect.Error);
            }

        }

        public static void ObtainLicences()
        {

            var options = new NeurotecLicenseOptions();

            NLicenseManager.TrialMode = options.TrialMode;

            var success = NLicense.ObtainComponents(options.Address, options.Port, options.ComponentsCsv);
            //var success = NLicense.ObtainComponents("10.1.1.176", 5001, components);

            if (!success) throw new Exception("Não foi possível obter a licença para os componentes: " + options.ComponentsCsv);
        }


        public static void ReleaseLicenses()
        {
            var options = new NeurotecLicenseOptions();
            NLicense.ReleaseComponents(options.ComponentsCsv);
        }
    }

    // Comentário: opções lidas do appsettings
    public sealed class NeurotecLicenseOptions
    {
        //public string Address { get; init; } = "/local";
        //public int Port { get; init; } = 6050;
        public string Address { get; init; } = "20.206.242.169";
        public int Port { get; init; } = 6000;
        public bool TrialMode { get; init; } = true;
        public string[] Components { get; init; } =
        [
            "Biometrics.FaceExtraction",
            "Biometrics.FaceMatching",
            "Biometrics.FaceDetection",
            "Biometrics.FaceSegmentsDetection"
        ];

        // Conveniência
        public string ComponentsCsv => string.Join(",", Components);
    }


}

