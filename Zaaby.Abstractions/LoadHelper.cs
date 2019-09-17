using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace Zaaby.Abstractions
{
    public static class LoadHelper
    {
        private static IList<Type> _allTypes;
        private static readonly object LockObj = new object();

        public static IList<Type> GetAllTypes()
        {
            if (_allTypes == null)
            {
                lock (LockObj)
                {
                    if (_allTypes == null)
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
                                    if (type.FullName != null && !typeDic.ContainsKey(type.FullName))
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

                        _allTypes = typeDic.Select(kv => kv.Value).ToList();
                    }
                }
            }

            return _allTypes;
        }
    }
}