using System;
using System.Reflection;
using Zaaby.Common;

namespace Zaaby
{
    public static partial class ZaabyHostExtensions
    {
        public static ZaabyHost FromAssemblyOf<T>(this ZaabyHost zaabyHost)
        {
            LoadHelper.FromAssemblyOf<T>();
            return zaabyHost;
        }

        public static ZaabyHost FromAssemblyOf<T0, T1>(this ZaabyHost zaabyHost)
        {
            LoadHelper.FromAssemblyOf<T0, T1>();
            return zaabyHost;
        }

        public static ZaabyHost FromAssemblyOf<T0, T1, T2>(this ZaabyHost zaabyHost)
        {
            LoadHelper.FromAssemblyOf<T0, T1, T2>();
            return zaabyHost;
        }

        public static ZaabyHost FromAssemblyOf<T0, T1, T2, T3>(this ZaabyHost zaabyHost)
        {
            LoadHelper.FromAssemblyOf<T0, T1, T2, T3>();
            return zaabyHost;
        }

        public static ZaabyHost FromAssemblyOf<T0, T1, T2, T3, T4>(this ZaabyHost zaabyHost)
        {
            LoadHelper.FromAssemblyOf<T0, T1, T2, T3, T4>();
            return zaabyHost;
        }

        public static ZaabyHost FromAssemblyOf<T0, T1, T2, T3, T4, T5>(this ZaabyHost zaabyHost)
        {
            LoadHelper.FromAssemblyOf<T0, T1, T2, T3, T4, T5>();
            return zaabyHost;
        }

        public static ZaabyHost FromAssemblyOf<T0, T1, T2, T3, T4, T5, T6>(this ZaabyHost zaabyHost)
        {
            LoadHelper.FromAssemblyOf<T0, T1, T2, T3, T4, T5, T6>();
            return zaabyHost;
        }

        public static ZaabyHost FromAssemblyOf<T0, T1, T2, T3, T4, T5, T6, T7>(this ZaabyHost zaabyHost)
        {
            LoadHelper.FromAssemblyOf<T0, T1, T2, T3, T4, T5, T6, T7>();
            return zaabyHost;
        }

        public static ZaabyHost FromAssemblyOf<T0, T1, T2, T3, T4, T5, T6, T7, T8>(this ZaabyHost zaabyHost)
        {
            LoadHelper.FromAssemblyOf<T0, T1, T2, T3, T4, T5, T6, T7, T8>();
            return zaabyHost;
        }

        public static ZaabyHost FromAssemblyOf<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9>(this ZaabyHost zaabyHost)
        {
            LoadHelper.FromAssemblyOf<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9>();
            return zaabyHost;
        }

        public static ZaabyHost FromAssemblyOf(this ZaabyHost zaabyHost, params Type[] types)
        {
            LoadHelper.FromAssemblyOf(types);
            return zaabyHost;
        }

        public static ZaabyHost FromAssemblies(this ZaabyHost zaabyHost, params Assembly[] assemblies)
        {
            LoadHelper.FromAssemblies(assemblies);
            return zaabyHost;
        }

        public static ZaabyHost FromAssemblyNames(this ZaabyHost zaabyHost, params AssemblyName[] assemblyNames)
        {
            LoadHelper.FromAssemblyNames(assemblyNames);
            return zaabyHost;
        }

        public static ZaabyHost FromDirectories(this ZaabyHost zaabyHost, params string[] directories)
        {
            LoadHelper.FromDirectories(directories);
            return zaabyHost;
        }
    }
}