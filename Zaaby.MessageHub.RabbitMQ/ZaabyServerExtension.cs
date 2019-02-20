using System;
using System.Collections.Generic;
using System.Linq;
using Zaabee.RabbitMQ.Abstractions;
using Zaaby.Abstractions;

namespace Zaaby.MessageHub.RabbitMQ
{
    public static class ZaabyServerExtension
    {
        internal static List<Type> AllTypes;
        internal static Type MessageHandlerInterfaceType;
        internal static Type MessageInterfaceType;
        internal static string HandleName;
        internal static ushort Prefetch;

        public static IZaabyServer UseMessageHub(this IZaabyServer zaabyServer,
            Func<IServiceProvider, IZaabeeRabbitMqClient> implementationFactory, Type messageHandlerInterfaceType,
            Type messageInterfaceType, string handleName, ushort prefetch)
        {
            AllTypes = AllTypes ?? zaabyServer.AllTypes;
            MessageHandlerInterfaceType = messageHandlerInterfaceType;
            MessageInterfaceType = messageInterfaceType;
            HandleName = handleName;
            Prefetch = prefetch;

            var messageHubInterfaceType = typeof(IZaabyMessageHub);
            var eventBusType =
                AllTypes.FirstOrDefault(type =>
                    messageHubInterfaceType.IsAssignableFrom(type) && type.IsClass);
            if (eventBusType == null) return zaabyServer;
            var messageConsumerTypes = AllTypes
                .Where(type => type.IsClass && messageHandlerInterfaceType.IsAssignableFrom(type)).ToList();
            if (!messageConsumerTypes.Any()) return zaabyServer;
            messageConsumerTypes.ForEach(messageConsumerType => zaabyServer.AddScoped(messageConsumerType));
            zaabyServer.AddSingleton(implementationFactory);
            zaabyServer.AddSingleton(messageHubInterfaceType, eventBusType);
            zaabyServer.RegisterServiceRunner(messageHubInterfaceType, eventBusType);
            return zaabyServer;
        }
    }
}