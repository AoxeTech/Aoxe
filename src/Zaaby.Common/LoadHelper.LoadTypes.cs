using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace Zaaby.Common
{
    public partial class LoadHelper
    {
        public static LoadMode LoadMode { get; set; } = LoadMode.LoadAll;
        private static readonly List<Type> ScanTypes = new();

        public static void FromAssemblyOf<T>() => FromAssemblyOf(typeof(T));

        public static void FromAssemblyOf(params Type[] types)
        {
            ScanTypes.AddRange(types
                .SelectMany(type => type.Assembly.GetTypes())
                .Where(p => !ScanTypes.Contains(p))
                .Distinct());
            LoadMode = LoadMode.LoadByScan;
        }

        public static IReadOnlyList<Type> LoadTypes() =>
            LoadMode is LoadMode.LoadByScan
                ? ScanTypes
                : AllTypesLazy.Value;

        public static IReadOnlyList<Type> LoadScanTypes() => ScanTypes;

        public static IReadOnlyList<Type> LoadAllTypes() => AllTypesLazy.Value;

        private static readonly Lazy<List<Type>> AllTypesLazy = new(() =>
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

            LoadMode = LoadMode.LoadAll;
            return types.Distinct().ToList();
        });
    }
}