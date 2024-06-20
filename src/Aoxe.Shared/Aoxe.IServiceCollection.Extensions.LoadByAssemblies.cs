namespace Aoxe.Shared;

public static partial class AoxeIServiceCollectionExtensions
{
    public static IServiceCollection FromAssemblyNames(
        this IServiceCollection services,
        params AssemblyName[] assemblyNames
    )
    {
        LoadHelper.FromAssemblyNames(assemblyNames);
        return services;
    }

    public static IServiceCollection FromAssemblies(
        this IServiceCollection services,
        params Assembly[] assemblies
    )
    {
        LoadHelper.FromAssemblies(assemblies);
        return services;
    }
}
