using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using Zaaby.Common;

namespace Zaaby.Client
{
    public static class AspDotNetCoreExtension
    {
        public static IServiceCollection UseZaabyClient(this IServiceCollection serviceCollection,
            Dictionary<string, List<string>> baseUrls)
        {
            if (baseUrls is null || baseUrls.Count <= 0) return serviceCollection;

            var allTypes = LoadHelper.GetAllTypes();

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
                .Where(@interface => @interface != null &&
                                     !string.IsNullOrWhiteSpace(@interface.Namespace) &&
                                     baseUrls.ContainsKey(@interface.Namespace))
                .Select(@interface => @interface.Namespace)
                .Distinct()
                .ToDictionary(k => k, v => baseUrls[v]));

            var methodInfo = client.GetType().GetMethod("GetService");
            if (methodInfo is null) return serviceCollection;
            
            foreach (var interfaceType in interfaceTypes)
                serviceCollection.AddScoped(interfaceType,
                    p => methodInfo.MakeGenericMethod(interfaceType).Invoke(client, null));

            return serviceCollection;
        }
    }
}