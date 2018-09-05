using System.Collections.Generic;
using System.Linq;
using Zaaby.Abstractions;

namespace Zaaby.Client
{
    public static class ZaabyServerExtension
    {
        public static IZaabyServer UseZaabyClient(this IZaabyServer zaabyServer,
            Dictionary<string, List<string>> baseUrls)
        {
            if (baseUrls == null) return zaabyServer;

            var interfaceTypes =
                zaabyServer.AllTypes.Where(type => type.IsInterface && baseUrls.ContainsKey(type.Namespace));
            var implementServiceTypes = zaabyServer.AllTypes
                .Where(type => type.IsClass && interfaceTypes.Any(i => i.IsAssignableFrom(type)))
                .ToList();
            interfaceTypes = interfaceTypes.Where(i =>
                implementServiceTypes.All(s => !i.IsAssignableFrom(s))).ToList();

            var client = new ZaabyClient(interfaceTypes
                .Where(@interface => baseUrls.ContainsKey(@interface.Namespace))
                .Select(@interface => @interface.Namespace)
                .Distinct()
                .ToDictionary(k => k, v => baseUrls[v]));
            
            var methodInfo = client.GetType().GetMethod("GetService");

            foreach (var interfaceType in interfaceTypes)
                zaabyServer.AddScoped(interfaceType,
                    p => methodInfo.MakeGenericMethod(interfaceType).Invoke(client, null));

            return zaabyServer;
        }
    }
}