using System.Linq;
using System.Reflection;

namespace Zaaby.Shared
{
    public static partial class LoadHelper
    {
        public static void FromAssemblyNames(params AssemblyName[] assemblyNames) =>
            FromAssemblies(assemblyNames.Select(Assembly.Load).ToArray());

        public static void FromAssemblies(params Assembly[] assemblies)
        {
            SpecifyTypes.AddRange(assemblies
                .SelectMany(assembly => assembly.GetTypes())
                .Where(p => !SpecifyTypes.Contains(p))
                .Distinct());
            LoadMode = LoadTypesMode.LoadBySpecify;
        }
    }
}