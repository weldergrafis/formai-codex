using System;
using System.Threading.Tasks;
using Azure.Messaging.ServiceBus;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace Resize;

public class ResizeFunction
{
    private readonly ILogger<ResizeFunction> _logger;

    public ResizeFunction(ILogger<ResizeFunction> logger)
    {
        _logger = logger;
    }

    [Function(nameof(ResizeFunction))]
    public async Task Run(
        [ServiceBusTrigger("resize", Connection = "ServiceBusConnection")]
        ServiceBusReceivedMessage message,
        ServiceBusMessageActions messageActions)
    {
        _logger.LogInformation("Message ID: {id}", message.MessageId);
        _logger.LogInformation("Message Body: {body}", message.Body);
        _logger.LogInformation("Message Content-Type: {contentType}", message.ContentType);

        // Complete the message
        await messageActions.CompleteMessageAsync(message);
    }
}