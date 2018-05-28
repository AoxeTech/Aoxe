using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;

namespace Zaaby.Client
{
    public static class AspDotNetCoreExtension
    {
        public static IServiceCollection UseZaabyClient(this IServiceCollection serviceCollection,
            Dictionary<string, List<string>> baseUrls, Func<Type, bool> applicationServiceInterfaceDefine = null)
        {
            if (baseUrls == null) return serviceCollection;

            var interfaces =
                ZaabyApplicationServiceTypeRepository
                    .GetZaabyApplicationServiceTypes(applicationServiceInterfaceDefine);

            var dynamicProxy = new ZaabyDynamicProxy(interfaces
                .Where(@interface => baseUrls.ContainsKey(@interface.Namespace))
                .Select(@interface => @interface.Namespace)
                .Distinct()
                .ToDictionary(k => k, v => baseUrls[v]));
            var dynamicType = dynamicProxy.GetType();
            var methodInfo = dynamicType.GetMethod("GetService");
            foreach (var interfaceType in interfaces)
            {
                var proxy = methodInfo.MakeGenericMethod(interfaceType).Invoke(dynamicProxy, null);
                serviceCollection.AddScoped(interfaceType, p => proxy);
            }

            return serviceCollection;
        }
    }
}