using System;
using System.Collections.Generic;
using System.Linq;
using Zaaby.Core;

namespace Zaaby.Client
{
    public static class ZaabyServerExtension
    {
        public static IZaabyServer UseZaabyApplicationServiceProxy(this IZaabyServer zaabyServer,
            Dictionary<string, List<string>> baseUrls,Func<Type, bool> applicationServiceInterfaceDefine = null)
        {
            if (baseUrls == null) return zaabyServer;

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
                zaabyServer.AddScoped(interfaceType, p => proxy);
            }

            return zaabyServer;
        }
    }
}