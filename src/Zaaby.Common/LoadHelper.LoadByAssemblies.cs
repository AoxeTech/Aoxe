using System.Linq;
using System.Reflection;

namespace Zaaby.Common
{
    public static partial class LoadHelper
    {
        public static void LoadByAssemblyNames(params AssemblyName[] assemblyNames) =>
            LoadByAssemblies(assemblyNames.Select(Assembly.Load).ToArray());

        public static void LoadByAssemblies(params Assembly[] assemblies)
        {
            SpecifyTypes.AddRange(assemblies
                .SelectMany(assembly => assembly.GetTypes())
                .Where(p => !SpecifyTypes.Contains(p))
                .Distinct());
            LoadMode = LoadTypesMode.LoadBySpecify;
        }
    }
}