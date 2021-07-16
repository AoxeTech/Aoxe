using System;
using System.Reflection;
using Zaaby.Common;

namespace Zaaby
{
    public static partial class ZaabyServerExtensions
    {
        public static ZaabyServer FromAssemblyOf<T>(this ZaabyServer zaabyServer)
        {
            LoadHelper.FromAssemblyOf<T>();
            return zaabyServer;
        }

        public static ZaabyServer FromAssemblyOf<T0, T1>(this ZaabyServer zaabyServer)
        {
            LoadHelper.FromAssemblyOf<T0, T1>();
            return zaabyServer;
        }

        public static ZaabyServer FromAssemblyOf<T0, T1, T2>(this ZaabyServer zaabyServer)
        {
            LoadHelper.FromAssemblyOf<T0, T1, T2>();
            return zaabyServer;
        }

        public static ZaabyServer FromAssemblyOf<T0, T1, T2, T3>(this ZaabyServer zaabyServer)
        {
            LoadHelper.FromAssemblyOf<T0, T1, T2, T3>();
            return zaabyServer;
        }

        public static ZaabyServer FromAssemblyOf<T0, T1, T2, T3, T4>(this ZaabyServer zaabyServer)
        {
            LoadHelper.FromAssemblyOf<T0, T1, T2, T3, T4>();
            return zaabyServer;
        }

        public static ZaabyServer FromAssemblyOf<T0, T1, T2, T3, T4, T5>(this ZaabyServer zaabyServer)
        {
            LoadHelper.FromAssemblyOf<T0, T1, T2, T3, T4, T5>();
            return zaabyServer;
        }

        public static ZaabyServer FromAssemblyOf<T0, T1, T2, T3, T4, T5, T6>(this ZaabyServer zaabyServer)
        {
            LoadHelper.FromAssemblyOf<T0, T1, T2, T3, T4, T5, T6>();
            return zaabyServer;
        }

        public static ZaabyServer FromAssemblyOf<T0, T1, T2, T3, T4, T5, T6, T7>(this ZaabyServer zaabyServer)
        {
            LoadHelper.FromAssemblyOf<T0, T1, T2, T3, T4, T5, T6, T7>();
            return zaabyServer;
        }

        public static ZaabyServer FromAssemblyOf<T0, T1, T2, T3, T4, T5, T6, T7, T8>(this ZaabyServer zaabyServer)
        {
            LoadHelper.FromAssemblyOf<T0, T1, T2, T3, T4, T5, T6, T7, T8>();
            return zaabyServer;
        }

        public static ZaabyServer FromAssemblyOf<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9>(this ZaabyServer zaabyServer)
        {
            LoadHelper.FromAssemblyOf<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9>();
            return zaabyServer;
        }

        public static ZaabyServer FromAssemblyOf(this ZaabyServer zaabyServer, params Type[] types)
        {
            LoadHelper.FromAssemblyOf(types);
            return zaabyServer;
        }

        public static ZaabyServer FromAssemblies(this ZaabyServer zaabyServer, params Assembly[] assemblies)
        {
            LoadHelper.FromAssemblies(assemblies);
            return zaabyServer;
        }

        public static ZaabyServer FromAssemblyNames(this ZaabyServer zaabyServer, params AssemblyName[] assemblyNames)
        {
            LoadHelper.FromAssemblyNames(assemblyNames);
            return zaabyServer;
        }

        public static ZaabyServer FromDirectories(this ZaabyServer zaabyServer, params string[] directories)
        {
            LoadHelper.FromDirectories(directories);
            return zaabyServer;
        }
    }
}