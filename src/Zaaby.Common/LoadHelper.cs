using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace Zaaby.Common
{
    public static class LoadHelper
    {
        public static List<Type> Types = new();
        private static bool _isLoaded;
        private static readonly object LockObj = new();

        public static void Scan(params Type[] types)
        {
            Types.AddRange(types.SelectMany(type => type.Assembly.ExportedTypes));
            Types = Types.Distinct().ToList();
        }

        public static void LoadAllTypes()
        {
            if (_isLoaded) return;
            lock (LockObj)
            {
                if (_isLoaded) return;
                var dir = Directory.GetCurrentDirectory();
                var files = new List<string>();

                files.AddRange(Directory.GetFiles(dir + @"/", "*.dll", SearchOption.AllDirectories));
                files.AddRange(Directory.GetFiles(dir + @"/", "*.exe", SearchOption.AllDirectories));
                files = files.Where(file => file != "Zaaby.dll").ToList();

                foreach (var file in files)
                {
                    try
                    {
                        Types.AddRange(Assembly.LoadFrom(file).ExportedTypes);
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

                Types = Types.Distinct().ToList();
                _isLoaded = true;
            }
        }
    }
}