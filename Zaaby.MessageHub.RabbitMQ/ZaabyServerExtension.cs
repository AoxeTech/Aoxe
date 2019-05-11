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

            var messageHandlerTypes = zaabyServer.AllTypes
                .Where(type => type.IsClass && messageHubConfig.MessageHandlerInterfaceType.IsAssignableFrom(type))
                .ToList();
            messageHandlerTypes.ForEach(messageHandlerType => zaabyServer.AddScoped(messageHandlerType));
            return zaabyServer;
        }
    }
}