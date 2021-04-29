using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace Zaaby.Common
{
    public partial class LoadHelper
    {
        public static LoadMode LoadMode { get; set; }
        private static readonly List<Type> _scanTypes = new();
        public static IReadOnlyList<Type> ScanTypes => _scanTypes;
        public static IReadOnlyList<Type> AllTypes { get; private set; }

        public static void FromAssemblyOf<T>() => FromAssemblyOf(typeof(T));

        public static void FromAssemblyOf(Type type)
        {
            LoadMode = LoadMode.LoadByScan;
            _scanTypes.AddRange(type.Assembly.GetTypes().Where(p => !ScanTypes.Contains(p)).Distinct());
        }

        public static IReadOnlyList<Type> LoadTypes() =>
            LoadMode is LoadMode.LoadByScan ? ScanTypes : AllTypes ?? LoadAllTypes();

        public static IReadOnlyList<Type> LoadAllTypes() => AllTypes ??= AllTypesLazy.Value;

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