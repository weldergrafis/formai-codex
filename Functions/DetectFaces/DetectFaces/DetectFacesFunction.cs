using Azure.Messaging.ServiceBus;
using DetectFaces.Services;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using Neurotec.Accelerator.Admin.Rest.Client;
using System;
using System.Threading.Tasks;

namespace DetectFaces;

public class DetectFacesFunction(ILogger<DetectFacesFunction> logger, StorageService storageService, NeurotecService neurotecService, FormaiApiClient apiClient)
//public class DetectFacesFunction(ILogger<DetectFacesFunction> logger, StorageService storageService, FormaiApiClient apiClient)
{

    public static bool IsInitialized = false;

    [Function(nameof(DetectFacesFunction))]
    public async Task Run(
        [ServiceBusTrigger("detect-faces", Connection = "ServiceBusConnection")]
        ServiceBusReceivedMessage message,
        ServiceBusMessageActions messageActions)
    {
        //if (!IsInitialized)
        //{
        //    try
        //    {
        //        NeurotecService.Initialize(); // Comentário: garante licença 1x por processo
        //        Console.WriteLine("NEUROTEC inicializado NOVO");
        //        logger.LogInformation("NEUROTEC inicializado NOVO");
        //    }
        //    catch (Exception ex)
        //    {
        //        logger.LogError($"NEUROTEC error - Exception: {ex.Message} - Inner: {ex.InnerException?.Message} - InnerInner: {ex.InnerException?.InnerException?.Message}");
        //        throw;
        //    }
            
        //    var asd = NeurotecService.CreateBiometricClient(); // Comentário: só cria após licenciar
        //    IsInitialized = true;
        //}

        var photoId = long.Parse(message.Body.ToString());
        var maxSide = 3072;

        var blobName = $"{maxSide}/{photoId}.jpg";
        var stream = await storageService.DownloadAsync(blobName);
        var faces = await neurotecService.DetectFacesAsync(stream);

        await apiClient.MarkFacesDetectedAsync(photoId);
    }
}