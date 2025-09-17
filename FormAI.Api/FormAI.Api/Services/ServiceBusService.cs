using Azure.Messaging.ServiceBus;

namespace FormAI.Api.Services
{
    public class ServiceBusService
    {
        const string SB_CONNECTION_STRING = "Endpoint=sb://formaisb.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=kRCqWycCrLuaPpD428rqU2Bi3fY4HqFb5+ASbHCPfyM=";
        const string QUEUE_NAME = "myqueue";

        // Comentário em português: envia UMA mensagem simples para a fila informada
        public static async Task SendOneMessageAsync(string body)
        {

            // Comentário em português: usa WebSockets (porta 443) para evitar bloqueios na 5671
            var options = new ServiceBusClientOptions
            {
                TransportType = ServiceBusTransportType.AmqpWebSockets
            };

            await using var client = new ServiceBusClient(SB_CONNECTION_STRING, options);
            ServiceBusSender sender = client.CreateSender(QUEUE_NAME);

            var message = new ServiceBusMessage(body) { ContentType = "text/plain" };

            await sender.SendMessageAsync(message);
            await sender.DisposeAsync();
        }
    }
}
