using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Zaaby.Common
{
    public static partial class LoadHelper
    {
        private static readonly List<Type> AssemblyTypes = new();

        public static IReadOnlyList<Type> LoadAssemblyTypes() => AssemblyTypes;

        public static void LoadByAssemblyNames(params AssemblyName[] assemblyNames) =>
            LoadByAssemblies(assemblyNames.Select(Assembly.Load).ToArray());

        public static void LoadByAssemblies(params Assembly[] assemblies)
        {
            AssemblyTypes.AddRange(assemblies
                .SelectMany(assembly => assembly.GetTypes())
                .Where(p => !ScanTypes.Contains(p))
                .Distinct());
            LoadMode = LoadTypesMode.LoadByAssemblies;
        }
    }
}