using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;

namespace Zaaby.Client
{
    public static class AspDotNetCoreExtension
    {
        public static IServiceCollection UseZaabyClient(this IServiceCollection serviceCollection,
            Dictionary<string, List<string>> baseUrls)
        {
            if (baseUrls == null || !baseUrls.Any()) return serviceCollection;

            var allTypes = GetAllTypes();

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
            if (methodInfo == null) return serviceCollection;
            
            foreach (var interfaceType in interfaceTypes)
                serviceCollection.AddScoped(interfaceType,
                    p => methodInfo.MakeGenericMethod(interfaceType).Invoke(client, null));

            return serviceCollection;
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
                catch (FileLoadException)
                {
                    // ignored
                }
            }

            return typeDic.Select(kv => kv.Value).ToList();
        }
    }
}