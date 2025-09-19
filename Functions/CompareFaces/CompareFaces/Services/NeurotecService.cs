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

