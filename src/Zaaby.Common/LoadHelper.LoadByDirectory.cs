using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace Zaaby.Common
{
    public static partial class LoadHelper
    {
        private static readonly Lazy<List<Type>> DirectoryTypesLazy = new(() =>
        {
            var dir = Directory.GetCurrentDirectory();
            var files = Directory.GetFiles(dir + @"/", "*.dll", SearchOption.AllDirectories)
                .Union(Directory.GetFiles(dir + @"/", "*.exe", SearchOption.AllDirectories));

            var result = files.Select(file =>
                {
                    try
                    {
                        return Assembly.LoadFrom(file).ExportedTypes;
                    }
                    catch (BadImageFormatException)
                    {
                        return null;
                    }
                    catch (FileLoadException)
                    {
                        return null;
                    }
                    catch (Exception)
                    {
                        return null;
                    }
                })
                .Where(types => types is not null)
                .SelectMany(types => types.Where(g => g is not null))
                .Distinct()
                .ToList();

            LoadMode = LoadTypesMode.LoadByDirectory;
            return result;
        });
    }
}