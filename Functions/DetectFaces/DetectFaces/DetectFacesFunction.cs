using Azure.Messaging.ServiceBus;
using DetectFaces.Services;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using Neurotec.Accelerator.Admin.Rest.Client;
using System;
using System.Threading.Tasks;

namespace DetectFaces;

public class DetectFacesFunction(ILogger<DetectFacesFunction> logger, StorageService storageService, NeurotecService neurotecService, FormaiApiClient apiClient)
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
        var faces = await neurotecService.DetectFacesAsync(stream);

        await apiClient.MarkFacesDetectedAsync(photoId);
    }
}