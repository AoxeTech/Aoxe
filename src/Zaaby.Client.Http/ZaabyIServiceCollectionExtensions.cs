using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using Zaaby.Common;

namespace Zaaby.Client.Http
{
    public static class ZaabyIServiceCollectionExtensions
    {
        public static IServiceCollection AddZaabyClient(this IServiceCollection services, Type serviceDefineType,
            Dictionary<string, string> baseUrls)
        {
            if (baseUrls is null || baseUrls.Count <= 0) return services;

            var typePairs = LoadHelper.GetByBaseType(serviceDefineType);
            var interfaceTypes = typePairs.Where(t => t.InterfaceType?.Namespace is not null
                                                      && t.ImplementationType is null
                                                      && baseUrls.ContainsKey(t.InterfaceType.Namespace))
                .Select(t => t.InterfaceType).ToList();

            var clientUrls = interfaceTypes
                .Where(@interface => @interface is not null
                                     && !string.IsNullOrWhiteSpace(@interface.Namespace)
                                     && baseUrls.ContainsKey(@interface.Namespace))
                .Select(@interface => @interface.Namespace)
                .Distinct()
                .ToDictionary(k => k, v => baseUrls[v]);

            foreach (var (@namespace, baseUrl) in clientUrls)
            {
                services.AddHttpClient(@namespace,
                    configureClient => { configureClient.BaseAddress = new Uri(baseUrl); });
            }

            services.AddScoped<ZaabyClient>();

            var methodInfo = typeof(ZaabyClient).GetMethod("GetService");
            if (methodInfo is null) throw new Exception("The Zaaby Client has no method witch named GetService.");
            foreach (var interfaceType in interfaceTypes)
            {
                services.AddScoped(interfaceType, _ => methodInfo.MakeGenericMethod(interfaceType).Invoke(services
                    .BuildServiceProvider().GetService<ZaabyClient>(), null));
            }

            return services;
        }
    }
}