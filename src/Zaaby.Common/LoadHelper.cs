using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace Zaaby.Common
{
    public static class LoadHelper
    {
        public static readonly List<Type> ScanTypes = new();

        public static void FromAssemblyOf(Type type) =>
            ScanTypes.AddRange(type.Assembly.GetTypes().Where(p => !ScanTypes.Contains(p)).Distinct());

        public static readonly List<Type> AllTypes = new Lazy<List<Type>>(() =>
        {
            var dir = Directory.GetCurrentDirectory();
            var files = new List<string>();

            files.AddRange(Directory.GetFiles(dir + @"/", "*.dll", SearchOption.AllDirectories));
            files.AddRange(Directory.GetFiles(dir + @"/", "*.exe", SearchOption.AllDirectories));
            files = files.Where(file => file != "Zaaby.dll").ToList();

            var types = new List<Type>();
            foreach (var file in files)
            {
                try
                {
                    types.AddRange(Assembly.LoadFrom(file).ExportedTypes);
                }
                catch (BadImageFormatException)
                {
                    // ignored
                }
                catch (FileLoadException)
                {
                    // ignored
                }
                catch (Exception)
                {
                    // ignored
                }
            }

            return types.Distinct().ToList();
        }).Value;

        public static List<TypePair> GetByBaseType<T>() => GetByBaseType(typeof(T));

        public static List<TypePair> GetByBaseType(Type baseType)
        {
            var types = AllTypes.Where(baseType.IsAssignableFrom).ToList();

            var interfaceTypes = types.Where(type => type.IsInterface && type != baseType).ToList();
            var classTypes = types.Where(type => type.IsClass).ToList();

            var result = interfaceTypes
                .Select(i => new TypePair(i, classTypes.FirstOrDefault(i.IsAssignableFrom))).ToList();

            result.AddRange(classTypes.Where(c => !result.Select(r => r.ImplementationType).Contains(c))
                .Select(c => new TypePair(null, c)));

            return result;
        }

        public static List<TypePair> GetByAttribute<TAttribute>() where TAttribute : Attribute =>
            GetByAttribute(typeof(TAttribute));

        public static List<TypePair> GetByAttribute(Type attributeType)
        {
            var types = AllTypes.Where(type => Attribute.GetCustomAttribute(type, attributeType, true) is not null)
                .ToList();

            var interfaceTypes = types.Where(type => type.IsInterface).ToList();
            var classTypes = types.Where(type => type.IsClass).ToList();

            var result = interfaceTypes
                .Select(i => new TypePair(i, classTypes.FirstOrDefault(i.IsAssignableFrom))).ToList();

            result.AddRange(classTypes.Where(c => !result.Select(r => r.ImplementationType).Contains(c))
                .Select(c => new TypePair(null, c)));

            return result;
        }
    }
}