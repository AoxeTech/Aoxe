using System.Collections.Generic;
using System.IO;
using Interfaces;
using Microsoft.Extensions.Configuration;
using Zaabee.RabbitMQ;
using Zaabee.RabbitMQ.Abstractions;
using Zaabee.RabbitMQ.Jil;
using Zaaby;
using Zaaby.Client;

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
//                .AddSingleton<IZaabeeRabbitMqClient>(p => new ZaabeeRabbitMqClient(rabbitMqConfig, new Serializer()))
                .UseZaabyClient(new Dictionary<string, List<string>>
                {
                    {"IAppleServices", new List<string> {"http://localhost:5001"}}
                })
                .RegisterServiceRunner<Runner>()
                .UseUrls("http://localhost:5002").Run();
        }
    }
}