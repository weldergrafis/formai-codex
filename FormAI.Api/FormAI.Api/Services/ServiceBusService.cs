using Azure.Messaging.ServiceBus;

namespace FormAI.Api.Services
{
    public enum Queue
    {
        Resize,
        DetectFaces,
    }

    public class ServiceBusService
    {
        const string SERVICEBUS_CONNECTION_STRING = "Endpoint=sb://formaisb.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=kRCqWycCrLuaPpD428rqU2Bi3fY4HqFb5+ASbHCPfyM=";

        private readonly ServiceBusClient _client;

        public ServiceBusService(IConfiguration configuration)
        {
            var options = new ServiceBusClientOptions { TransportType = ServiceBusTransportType.AmqpWebSockets };
            _client = new ServiceBusClient(SERVICEBUS_CONNECTION_STRING, options);
        }

        // Comentário em português: envia UMA mensagem simples para a fila informada
        public async Task SendMessageAsync(string body, Queue queue)
        {
            var queueName = GetQueueName(queue);
            var sender = _client.CreateSender(queueName);

            var message = new ServiceBusMessage(body) { ContentType = "text/plain" };

            await sender.SendMessageAsync(message);
            await sender.DisposeAsync();
        }


        public static string GetQueueName(Queue queue)
        {
            if (queue == Queue.Resize) return "resize";
            else if (queue == Queue.DetectFaces) return "detect-faces";
            else throw new Exception($"Queue não suportada: {queue}");
        }
    }
}