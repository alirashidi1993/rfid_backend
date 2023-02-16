using Microsoft.AspNetCore.SignalR;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using Sgms.Backend.Hubs;
using System.Text;

namespace Sgms.Backend
{
    public class WinServiceCardListenerClient : IDisposable
    {
        ConnectionFactory factory;
        IConnection connection;
        IModel? channel;
        private readonly IHubContext<CardHub> hubContext;

        public WinServiceCardListenerClient(IHubContext<CardHub> hubContext)
        {
            factory = new ConnectionFactory { HostName = "localhost" };
            connection = factory.CreateConnection();
            channel = connection.CreateModel();
            this.hubContext = hubContext;
        }

        public void Start()
        {

            channel.ExchangeDeclare(exchange: PcscCardReader.Constants.ExchangeName, type: ExchangeType.Direct);
            var queueName = channel.QueueDeclare().QueueName;

            channel.QueueBind(queue: queueName,
                              exchange: PcscCardReader.Constants.ExchangeName,
                              routingKey: PcscCardReader.Constants.RoutingKey);

            var consumer = new EventingBasicConsumer(channel);
            consumer.Received += async (model, ea) =>
           {
               byte[] body = ea.Body.ToArray();
               var message = Encoding.UTF8.GetString(body);
               await hubContext.Clients.All.SendAsync("AccessGranted", "SPECFRY!");
            };
            channel.BasicConsume(queue: queueName,
                                 autoAck: true,
                                 consumer: consumer);
        }
        public void Dispose()
        {
            if (connection is not null)
                connection.Dispose();
        }
    }
}
