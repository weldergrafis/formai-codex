using Azure.Messaging.ServiceBus;
using DetectFaces;
using DetectFaces.Services;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;
using static DetectFaces.FormaiApiClient;

namespace CompareFaces;

public class CompareFacesFunction(ILogger<CompareFacesFunction> logger, FormaiApiClient apiClient)
{
    [Function(nameof(CompareFacesFunction))]
    public async Task Run(
        [ServiceBusTrigger("compare-faces", Connection = "ServiceBusConnection")]
        ServiceBusReceivedMessage message,
        ServiceBusMessageActions messageActions)
    {
        var comparisonOrder = long.Parse(message.Body.ToString());

        var request = new FacesTemplatesRequest { End = comparisonOrder };
        var templates = await apiClient.GetFacesTemplatesAsync(request);

        // Complete the message
        await messageActions.CompleteMessageAsync(message);
    }
}