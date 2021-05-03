using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace Zaaby.Common
{
    public static partial class LoadHelper
    {
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