using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace Zaaby.Client
{
    internal static class ServiceTypeRepository
    {
        private static readonly List<Type> AllTypes;

        static ServiceTypeRepository()
        {
            AllTypes = GetAllTypes();
        }

        internal static List<Type> GetZaabyApplicationServiceTypes(
            Func<Type, bool> applicationServiceInterfaceDefine)
        {
            var allInterfaces = AllTypes.Where(type => type.IsInterface);

            var serviceInterfaces = allInterfaces.Where(applicationServiceInterfaceDefine);
            var implementServices = AllTypes
                .Where(type => type.IsClass && serviceInterfaces.Any(i => i.IsAssignableFrom(type)))
                .ToList();

            return serviceInterfaces.Where(i =>
                implementServices.All(s => !i.IsAssignableFrom(s))).ToList();
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