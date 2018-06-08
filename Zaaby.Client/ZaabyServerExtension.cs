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

            var interfaces =
                ServiceTypeRepository.GetZaabyApplicationServiceTypes(type => baseUrls.ContainsKey(type.Namespace));

            var client = new ZaabyClient(interfaces
                .Where(@interface => baseUrls.ContainsKey(@interface.Namespace))
                .Select(@interface => @interface.Namespace)
                .Distinct()
                .ToDictionary(k => k, v => baseUrls[v]));
            var dynamicType = client.GetType();
            var methodInfo = dynamicType.GetMethod("GetService");
            foreach (var interfaceType in interfaces)
            {
                var proxy = methodInfo.MakeGenericMethod(interfaceType).Invoke(client, null);
                zaabyServer.AddScoped(interfaceType, p => proxy);
            }

            return zaabyServer;
        }
    }
}