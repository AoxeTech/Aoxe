using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using Zaaby.Common;

namespace Zaaby.Client
{
    public static class ZaabyIServiceCollectionExtensions
    {
        public static IServiceCollection UseZaabyClient(this IServiceCollection services,Type serviceDefineType,
            Dictionary<string, List<string>> baseUrls)
        {
            if (baseUrls is null || baseUrls.Count <= 0) return services;

            var typePairs = LoadHelper.GetByBaseType(serviceDefineType);
            var interfaceTypes = typePairs.Where(t => t.InterfaceType?.Namespace is not null
                                                      && t.ImplementationType is null
                                                      && baseUrls.ContainsKey(t.InterfaceType.Namespace))
                .Select(t => t.InterfaceType).ToList();

            var client = new ZaabyClient(interfaceTypes
                .Where(@interface => @interface is not null
                                     && !string.IsNullOrWhiteSpace(@interface.Namespace)
                                     && baseUrls.ContainsKey(@interface.Namespace))
                .Select(@interface => @interface.Namespace)
                .Distinct()
                .ToDictionary(k => k, v => baseUrls[v]));

            var methodInfo = client.GetType().GetMethod("GetService");
            if (methodInfo is null) return services;

            foreach (var interfaceType in interfaceTypes)
                services.AddScoped(interfaceType,
                    p => methodInfo.MakeGenericMethod(interfaceType).Invoke(client, null));

            return services;
        }
    }
}