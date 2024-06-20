namespace Aoxe;

public static partial class AoxeHostExtensions
{
    public static AoxeHost FromAssemblyOf<T>(this AoxeHost AoxeHost)
    {
        LoadHelper.FromAssemblyOf<T>();
        return AoxeHost;
    }

    public static AoxeHost FromAssemblyOf<T0, T1>(this AoxeHost AoxeHost)
    {
        LoadHelper.FromAssemblyOf<T0, T1>();
        return AoxeHost;
    }

    public static AoxeHost FromAssemblyOf<T0, T1, T2>(this AoxeHost AoxeHost)
    {
        LoadHelper.FromAssemblyOf<T0, T1, T2>();
        return AoxeHost;
    }

    public static AoxeHost FromAssemblyOf<T0, T1, T2, T3>(this AoxeHost AoxeHost)
    {
        LoadHelper.FromAssemblyOf<T0, T1, T2, T3>();
        return AoxeHost;
    }

    public static AoxeHost FromAssemblyOf<T0, T1, T2, T3, T4>(this AoxeHost AoxeHost)
    {
        LoadHelper.FromAssemblyOf<T0, T1, T2, T3, T4>();
        return AoxeHost;
    }

    public static AoxeHost FromAssemblyOf<T0, T1, T2, T3, T4, T5>(this AoxeHost AoxeHost)
    {
        LoadHelper.FromAssemblyOf<T0, T1, T2, T3, T4, T5>();
        return AoxeHost;
    }

    public static AoxeHost FromAssemblyOf<T0, T1, T2, T3, T4, T5, T6>(this AoxeHost AoxeHost)
    {
        LoadHelper.FromAssemblyOf<T0, T1, T2, T3, T4, T5, T6>();
        return AoxeHost;
    }

    public static AoxeHost FromAssemblyOf<T0, T1, T2, T3, T4, T5, T6, T7>(this AoxeHost AoxeHost)
    {
        LoadHelper.FromAssemblyOf<T0, T1, T2, T3, T4, T5, T6, T7>();
        return AoxeHost;
    }

    public static AoxeHost FromAssemblyOf<T0, T1, T2, T3, T4, T5, T6, T7, T8>(
        this AoxeHost AoxeHost
    )
    {
        LoadHelper.FromAssemblyOf<T0, T1, T2, T3, T4, T5, T6, T7, T8>();
        return AoxeHost;
    }

    public static AoxeHost FromAssemblyOf<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9>(
        this AoxeHost AoxeHost
    )
    {
        LoadHelper.FromAssemblyOf<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9>();
        return AoxeHost;
    }

    public static AoxeHost FromAssemblyOf(this AoxeHost AoxeHost, params Type[] types)
    {
        LoadHelper.FromAssemblyOf(types);
        return AoxeHost;
    }

    public static AoxeHost FromAssemblies(this AoxeHost AoxeHost, params Assembly[] assemblies)
    {
        LoadHelper.FromAssemblies(assemblies);
        return AoxeHost;
    }

    public static AoxeHost FromAssemblyNames(
        this AoxeHost AoxeHost,
        params AssemblyName[] assemblyNames
    )
    {
        LoadHelper.FromAssemblyNames(assemblyNames);
        return AoxeHost;
    }

    public static AoxeHost FromDirectories(this AoxeHost AoxeHost, params string[] directories)
    {
        LoadHelper.FromDirectories(directories);
        return AoxeHost;
    }
}
