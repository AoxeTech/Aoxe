namespace Aoxe;

public static partial class AoxeHostExtensions
{
    public static AoxeHost FromAssemblyOf<T>(this AoxeHost aoxeHost)
    {
        LoadHelper.FromAssemblyOf<T>();
        return aoxeHost;
    }

    public static AoxeHost FromAssemblyOf<T0, T1>(this AoxeHost aoxeHost)
    {
        LoadHelper.FromAssemblyOf<T0, T1>();
        return aoxeHost;
    }

    public static AoxeHost FromAssemblyOf<T0, T1, T2>(this AoxeHost aoxeHost)
    {
        LoadHelper.FromAssemblyOf<T0, T1, T2>();
        return aoxeHost;
    }

    public static AoxeHost FromAssemblyOf<T0, T1, T2, T3>(this AoxeHost aoxeHost)
    {
        LoadHelper.FromAssemblyOf<T0, T1, T2, T3>();
        return aoxeHost;
    }

    public static AoxeHost FromAssemblyOf<T0, T1, T2, T3, T4>(this AoxeHost aoxeHost)
    {
        LoadHelper.FromAssemblyOf<T0, T1, T2, T3, T4>();
        return aoxeHost;
    }

    public static AoxeHost FromAssemblyOf<T0, T1, T2, T3, T4, T5>(this AoxeHost aoxeHost)
    {
        LoadHelper.FromAssemblyOf<T0, T1, T2, T3, T4, T5>();
        return aoxeHost;
    }

    public static AoxeHost FromAssemblyOf<T0, T1, T2, T3, T4, T5, T6>(this AoxeHost aoxeHost)
    {
        LoadHelper.FromAssemblyOf<T0, T1, T2, T3, T4, T5, T6>();
        return aoxeHost;
    }

    public static AoxeHost FromAssemblyOf<T0, T1, T2, T3, T4, T5, T6, T7>(this AoxeHost aoxeHost)
    {
        LoadHelper.FromAssemblyOf<T0, T1, T2, T3, T4, T5, T6, T7>();
        return aoxeHost;
    }

    public static AoxeHost FromAssemblyOf<T0, T1, T2, T3, T4, T5, T6, T7, T8>(
        this AoxeHost aoxeHost
    )
    {
        LoadHelper.FromAssemblyOf<T0, T1, T2, T3, T4, T5, T6, T7, T8>();
        return aoxeHost;
    }

    public static AoxeHost FromAssemblyOf<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9>(
        this AoxeHost aoxeHost
    )
    {
        LoadHelper.FromAssemblyOf<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9>();
        return aoxeHost;
    }

    public static AoxeHost FromAssemblyOf(this AoxeHost aoxeHost, params Type[] types)
    {
        LoadHelper.FromAssemblyOf(types);
        return aoxeHost;
    }

    public static AoxeHost FromAssemblies(this AoxeHost aoxeHost, params Assembly[] assemblies)
    {
        LoadHelper.FromAssemblies(assemblies);
        return aoxeHost;
    }

    public static AoxeHost FromAssemblyNames(
        this AoxeHost AoxeHost,
        params AssemblyName[] assemblyNames
    )
    {
        LoadHelper.FromAssemblyNames(assemblyNames);
        return AoxeHost;
    }

    public static AoxeHost FromDirectories(this AoxeHost aoxeHost, params string[] directories)
    {
        LoadHelper.FromDirectories(directories);
        return aoxeHost;
    }
}
