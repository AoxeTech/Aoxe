using System.Collections.Generic;
using System.IO;
using Interfaces;
using Microsoft.Extensions.Configuration;
using Zaabee.RabbitMQ;
using Zaabee.RabbitMQ.Abstractions;
using Zaaby;
using Zaaby.Client;
using Zaaby.MessageHub.RabbitMQ;
using Serializer = Zaabee.RabbitMQ.Jil.Serializer;

namespace AppleHost
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
                    {"IBananaServices", new List<string> {"http://localhost:5001"}}
                })
                .UseUrls("http://localhost:5000").Run();
        }
    }
}