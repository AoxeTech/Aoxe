using System.Collections.Generic;
using System.IO;
using Interfaces;
using Microsoft.Extensions.Configuration;
using Zaabee.RabbitMQ;
using Zaabee.RabbitMQ.Jil;
using Zaaby;
using Zaaby.Client;
using Zaaby.MessageHub.RabbitMQ;

namespace BananaHost
{
    class Program
    {
        static void Main(string[] args)
        {
            var configBuilder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("RabbitMQ.json", true, true);
            var config = configBuilder.Build();

            var rabbitMqConfig = config.GetSection("ZaabeeRabbitMQ").Get<MqConfig>();
            ZaabyServer.GetInstance()
                .UseZaabyServer<ITest>()
                .UseMessageHub(p => new ZaabeeRabbitMqClient(rabbitMqConfig, new Serializer()),
                    typeof(IConsumer),
                    typeof(IMessage),
                    "Consume", 100)
                .UseZaabyClient(new Dictionary<string, List<string>>
                {
                    {"IAppleServices", new List<string> {"http://localhost:5000"}}
                })
                .UseUrls("http://localhost:5001").Run();
        }
    }
}