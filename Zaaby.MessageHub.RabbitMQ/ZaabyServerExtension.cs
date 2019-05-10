using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Zaabee.RabbitMQ.Abstractions;
using Zaaby.Abstractions;

namespace Zaaby.MessageHub.RabbitMQ
{
    public static class ZaabyServerExtension
    {
        public static IZaabyServer UseRabbitMqMessageHub(this IZaabyServer zaabyServer,
            Func<IServiceProvider, IZaabeeRabbitMqClient> rmqClientFactory,
            MessageHubConfig messageHubConfig)
        {
            zaabyServer.AddSingleton(rmqClientFactory);
            zaabyServer.AddSingleton(p => messageHubConfig);
            zaabyServer.RegisterServiceRunner<IZaabyMessageHub, ZaabyMessageHub>();

            var messageHandlerTypes = GetAllTypes()
                .Where(type => type.IsClass && messageHubConfig.MessageHandlerInterfaceType.IsAssignableFrom(type))
                .ToList();
            messageHandlerTypes.ForEach(messageHandlerType => zaabyServer.AddScoped(messageHandlerType));
            return zaabyServer;
        }

        private static List<Type> GetAllTypes()
        {
            var dir = Directory.GetCurrentDirectory();
            var files = new List<string>();

            files.AddRange(Directory.GetFiles(dir + @"/", "*.dll", SearchOption.AllDirectories));
            files.AddRange(Directory.GetFiles(dir + @"/", "*.exe", SearchOption.AllDirectories));

            var typeDic = new Dictionary<string, Type>();

            foreach (var file in files)
            {
                try
                {
                    foreach (var type in Assembly.LoadFrom(file).GetTypes())
                        if (!typeDic.ContainsKey(type.FullName))
                            typeDic.Add(type.FullName, type);
                }
                catch
                {
                    // ignored
                }
            }

            return typeDic.Select(kv => kv.Value).ToList();
        }
    }
}