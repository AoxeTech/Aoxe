using System;
using System.Collections.Generic;
using System.Linq;
using Zaaby.Abstractions;

namespace Zaaby.MessageHub
{
    public static class ZaabyServerExtension
    {
        internal static List<Type> AllTypes;

        public static IZaabyServer UseMessageHub<TMessage>(this IZaabyServer zaabyServer)
        {
            AllTypes = zaabyServer.AllTypes;
            var interfaceType = typeof(IZaabyMessageHub);
            var eventBusType =
                AllTypes.FirstOrDefault(type => interfaceType.IsAssignableFrom(type) && type.IsClass);
            if (eventBusType == null) return zaabyServer;
            zaabyServer.AddSingleton(interfaceType, eventBusType);
            zaabyServer.RegisterServiceRunner(interfaceType, eventBusType);
            return zaabyServer;
        }
    }
}