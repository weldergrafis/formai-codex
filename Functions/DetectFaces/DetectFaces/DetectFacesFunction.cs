using Azure.Messaging.ServiceBus;
using DetectFaces.Services;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using Neurotec.Accelerator.Admin.Rest.Client;
using Neurotec.Biometrics;
using System;
using System.Threading.Tasks;
using static DetectFaces.Services.NeurotecService;

namespace DetectFaces;

public class DetectFacesFunction(ILogger<DetectFacesFunction> logger, StorageService storageService, NeurotecService neurotecService, FormaiApiClient apiClient)
//public class DetectFacesFunction(ILogger<DetectFacesFunction> logger, StorageService storageService, FormaiApiClient apiClient)
{

    [Function(nameof(DetectFacesFunction))]
    public async Task Run(
        [ServiceBusTrigger("detect-faces", Connection = "ServiceBusConnection")]
        ServiceBusReceivedMessage message,
        ServiceBusMessageActions messageActions)
    {
        var photoId = long.Parse(message.Body.ToString());
        var maxSide = 3072;

        var blobName = $"{maxSide}/{photoId}.jpg";
        var stream = await storageService.DownloadAsync(blobName);
        var detectFacesResult = await neurotecService.DetectFacesAsync(stream);

        //double imageWidth = detectFacesResult.ImageSize.Width;
        //double imageHeight = detectFacesResult.ImageSize.Height;

        var facesDto = detectFacesResult.Faces
            .Select(x=>FormaiApiClient.ConvertNeurotecFaceToDto(x, detectFacesResult.ImageSize.Width, detectFacesResult.ImageSize.Height))
            .ToList();

        for (int i = 0; i < facesDto.Count; i++)
        {
            var faceDto = facesDto[i];
            faceDto.NeurotecOrder = i;
        }

        await apiClient.CreateFaces(photoId, facesDto);

        await messageActions.CompleteMessageAsync(message);
    }
}