using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Zaaby.Core;
using Zaaby.Core.Application;

namespace Zaaby.ApplicationServiceProxy
{
    public static class ZaabyServerExtension
    {
        private static readonly List<Type> AllTypes;

        static ZaabyServerExtension()
        {
            AllTypes = GetAllTypes();
        }

        public static IZaabyServer UseZaabyApplicationServiceProxy(this IZaabyServer zaabyServer,
            Dictionary<string, List<string>> baseUrls = null, Func<Type, bool> applicationServiceInterfaceDefine = null)
        {
            var allInterfaces = AllTypes.Where(type => type.IsInterface);

            var serviceInterfaces = applicationServiceInterfaceDefine != null
                ? allInterfaces.Where(applicationServiceInterfaceDefine)
                : allInterfaces.Where(type =>
                    typeof(IApplicationService).IsAssignableFrom(type) &&
                    type != typeof(IApplicationService));
            var implementServices = AllTypes
                .Where(type => type.IsClass && serviceInterfaces.Any(i => i.IsAssignableFrom(type)))
                .ToList();

            if (baseUrls != null)
            {
                var interfaces = serviceInterfaces.Where(i =>
                    implementServices.All(s => !i.IsAssignableFrom(s))).ToList();

                var dynamicProxy = new ZaabyDynamicProxy(interfaces
                    .Where(@interface => baseUrls.ContainsKey(@interface.Namespace))
                    .Select(@interface => @interface.Namespace)
                    .Distinct()
                    .ToDictionary(k => k, v => baseUrls[v]));
                var type = dynamicProxy.GetType();
                var methodInfo = type.GetMethod("GetService");
                foreach (var interfaceType in interfaces)
                {
                    var proxy = methodInfo.MakeGenericMethod(interfaceType).Invoke(dynamicProxy, null);
                    zaabyServer.AddScoped(interfaceType, p => proxy);
                }
            }

            return zaabyServer;
        }

        private static List<Type> GetAllTypes()
        {
            var dir = Directory.GetCurrentDirectory();
            var files = new List<string>();

            files.AddRange(Directory.GetFiles(dir + @"/", "*.dll", SearchOption.AllDirectories));
            files.AddRange(Directory.GetFiles(dir + @"/", "*.exe", SearchOption.AllDirectories));

            var typeDic = new Dictionary<string, Type>();

            foreach (var file in files)
            {
                try
                {
                    foreach (var type in Assembly.LoadFrom(file).GetTypes())
                        if (!typeDic.ContainsKey(type.FullName))
                            typeDic.Add(type.FullName, type);
                }
                catch (BadImageFormatException)
                {
                    // ignored
                }
            }

            return typeDic.Select(kv => kv.Value).ToList();
        }
    }
}