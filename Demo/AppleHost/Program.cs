using System.Collections.Generic;
using System.IO;
using Interfaces;
using Microsoft.Extensions.Configuration;
using Zaabee.RabbitMQ;
using Zaabee.RabbitMQ.SystemTextJson;
using Zaaby;
using Zaaby.Client;
using Zaaby.MessageHub.RabbitMQ;

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

            //var rabbitMqConfig = config.GetSection("ZaabeeRabbitMQ").Get<MqConfig>();

            ZaabyServer.GetInstance()
                .UseZaabyServer<ITest>()
                //.UseRabbitMqMessageHub(p => new ZaabeeRabbitMqClient(rabbitMqConfig, new Serializer()),
                //    new MessageHubConfig
                //    {
                //        HandleName = "Consume",
                //        MessageHandlerInterfaceType = typeof(IConsumer),
                //        MessageInterfaceType = typeof(IMessage),
                //        Prefetch = 100
                //    })
                .UseZaabyClient(new Dictionary<string, List<string>>
                {
                    {"IBananaServices", new List<string> {"http://localhost:5002"}}
                })
                .UseUrls("http://localhost:5001").Run();
        }
    }
}