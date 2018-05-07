using System.Collections.Generic;
using System.IO;
using Microsoft.Extensions.Configuration;
using Zaabee.Mongo;
using Zaabee.Mongo.Common;
using Zaabee.Mongo.Core;
using Zaaby;
using Zaaby.Cache.RedisProvider;
using Zaaby.MessageBus.Core;
using Zaaby.MessageBus.RabbitMqProvider;
using Zaaby.MessageBus.RabbitMqProvider.Json;

namespace ShippingHost
{
    class Program
    {
        static void Main(string[] args)
        {
            var configBuilder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("ApplicationService.json", true, true)
                .AddJsonFile("Mongo.json", true, true)
                .AddJsonFile("RabbitMQ.json", true, true)
                .AddJsonFile("Redis.json", true, true);
            var config = configBuilder.Build();

            var appServiceConfig = config.GetSection("ZaabyApplication").Get<Dictionary<string, List<string>>>();
            var mongoConfig = config.GetSection("ZaabeMongo").Get<MongoDbConfiger>();
            var rabbitmqConfig = config.GetSection("ZaabyRabbitMQ").Get<MqConfig>();
            var redisConfig = config.GetSection("ZaabyRedis").Get<RedisConfig>();

            ZaabyServer.GetInstance()
                .UseZaabyRepository()
                .UseZaabyDomainService()
                .UseZaabyApplicationService(appServiceConfig)
                .AddSingleton<IMongoDbRepository>(p => new MongoDbRepository(mongoConfig))
                .AddSingleton<IHandler, ZabbyRabbitMqClient>(p =>
                    new ZabbyRabbitMqClient(rabbitmqConfig, new Serializer()))
                .AddSingleton<Zaaby.Cache.Core.IHandler, ZaabyRedisClient>(p =>
                    new ZaabyRedisClient(redisConfig, new Zaaby.Cache.RedisProvider.Protobuf.Serializer()))
                .UseUrls("http://*:5002")
                .Run();
        }
    }
}