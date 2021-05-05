using System.Reflection;
using Microsoft.Extensions.DependencyInjection;

namespace Zaaby.Common
{
    public static partial class ZaabyIServiceCollectionExtensions
    {
        public static IServiceCollection FromAssemblyNames(this IServiceCollection services,
            params AssemblyName[] assemblyNames)
        {
            LoadHelper.FromAssemblyNames(assemblyNames);
            return services;
        }

        public static IServiceCollection FromAssemblies(this IServiceCollection services, params Assembly[] assemblies)
        {
            LoadHelper.FromAssemblies(assemblies);
            return services;
        }
    }
}