using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using Zaaby.Common;

namespace Zaaby.Client
{
    public static class ZaabyIServiceCollectionExtensions
    {
        public static IServiceCollection UseZaabyClient(this IServiceCollection services,
            Dictionary<string, List<string>> baseUrls)
        {
            if (baseUrls is null || baseUrls.Count <= 0) return services;

            if (!LoadHelper.Types.Any()) LoadHelper.LoadAllTypes();
            var allTypes = LoadHelper.Types;

            var interfaceTypes = allTypes.Where(type =>
                    type.IsInterface &&
                    !string.IsNullOrWhiteSpace(type.Namespace) &&
                    baseUrls.ContainsKey(type.Namespace))
                .ToList();
            var implementServiceTypes =
                allTypes.Where(type => type.IsClass && interfaceTypes.Any(i => i.IsAssignableFrom(type))).ToList();
            interfaceTypes = interfaceTypes.Where(i =>
                implementServiceTypes.All(s => !i.IsAssignableFrom(s))).ToList();

            var client = new ZaabyClient(interfaceTypes
                .Where(@interface => @interface is not null &&
                                     !string.IsNullOrWhiteSpace(@interface.Namespace) &&
                                     baseUrls.ContainsKey(@interface.Namespace))
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