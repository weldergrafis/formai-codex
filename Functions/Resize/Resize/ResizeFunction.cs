using Azure.Messaging.ServiceBus;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using Resize.Helpers;
using Resize.Services;
using SixLabors.ImageSharp;
using System;
using System.Threading.Tasks;

namespace Resize;

public class ResizeFunction(ILogger<ResizeFunction> logger, StorageService storageService, FormaiApiClient apiClient)
{


    [Function(nameof(ResizeFunction))]
    public async Task Run(
        [ServiceBusTrigger("resize", Connection = "ServiceBusConnection")]
        ServiceBusReceivedMessage message,
        ServiceBusMessageActions messageActions)
    {

        var start = DateTime.Now;

        // Comentário: corpo da mensagem deve conter o ID da foto (ex.: $"{photo.Id}")
        var photoId = long.Parse(message.Body.ToString());

        // Baixa o original
        var originalBlobPath = $"originals/{photoId}.jpg";
        var originalStream = await storageService.DownloadAsync(originalBlobPath);

        // Testes locais quando necessários
        //var originalStream = $@"\\srvdados\Pasta Pública\Jeferson\JEIVISON\Casamento.jpg";

        using var originalImage = await Image.LoadAsync(originalStream);

        // Descarta imeditamente depois de ser usada para não ocupar memória
        originalStream.Dispose();

        var maxSides = new List<int> { 512, 3072 };

        foreach (var maxSide in maxSides)
        {
            var resizedBlobName = $"{maxSide}/{photoId}.jpg";
            var resizedImage = ImageHelper.Resize(originalImage, maxSide);
            using var resizedStream = await ImageHelper.ToStreamAsync(resizedImage, 90);
            await storageService.UploadAsync(resizedBlobName, resizedStream);

            // Testes locais
            //using var fileStream = new FileStream($"c:/temp/jeivison/{photoId} - {maxSide}.jpg", FileMode.Create, FileAccess.Write, FileShare.None);
            //await resizedStream.CopyToAsync(fileStream);
        }

        // Complete the message
        await messageActions.CompleteMessageAsync(message);

        await apiClient.MarkResizedAsync(photoId);

        var elapsed = DateTime.Now - start;
        Console.WriteLine($"Tempo: {elapsed}");
    }
}