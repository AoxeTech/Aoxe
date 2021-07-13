using System;
using System.Reflection;
using Zaaby.Common;

namespace Zaaby
{
    public static partial class ZaabyServerExtensions
    {
        public static ZaabyServer FromAssemblyOf<T>(this ZaabyServer server)
        {
            LoadHelper.FromAssemblyOf<T>();
            return server;
        }

        public static ZaabyServer FromAssemblyOf<T0, T1>(this ZaabyServer server)
        {
            LoadHelper.FromAssemblyOf<T0, T1>();
            return server;
        }

        public static ZaabyServer FromAssemblyOf<T0, T1, T2>(this ZaabyServer server)
        {
            LoadHelper.FromAssemblyOf<T0, T1, T2>();
            return server;
        }

        public static ZaabyServer FromAssemblyOf<T0, T1, T2, T3>(this ZaabyServer server)
        {
            LoadHelper.FromAssemblyOf<T0, T1, T2, T3>();
            return server;
        }

        public static ZaabyServer FromAssemblyOf<T0, T1, T2, T3, T4>(this ZaabyServer server)
        {
            LoadHelper.FromAssemblyOf<T0, T1, T2, T3, T4>();
            return server;
        }

        public static ZaabyServer FromAssemblyOf<T0, T1, T2, T3, T4, T5>(this ZaabyServer server)
        {
            LoadHelper.FromAssemblyOf<T0, T1, T2, T3, T4, T5>();
            return server;
        }

        public static ZaabyServer FromAssemblyOf<T0, T1, T2, T3, T4, T5, T6>(this ZaabyServer server)
        {
            LoadHelper.FromAssemblyOf<T0, T1, T2, T3, T4, T5, T6>();
            return server;
        }

        public static ZaabyServer FromAssemblyOf<T0, T1, T2, T3, T4, T5, T6, T7>(this ZaabyServer server)
        {
            LoadHelper.FromAssemblyOf<T0, T1, T2, T3, T4, T5, T6, T7>();
            return server;
        }

        public static ZaabyServer FromAssemblyOf<T0, T1, T2, T3, T4, T5, T6, T7, T8>(this ZaabyServer server)
        {
            LoadHelper.FromAssemblyOf<T0, T1, T2, T3, T4, T5, T6, T7, T8>();
            return server;
        }

        public static ZaabyServer FromAssemblyOf<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9>(this ZaabyServer server)
        {
            LoadHelper.FromAssemblyOf<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9>();
            return server;
        }

        public static ZaabyServer FromAssemblyOf(this ZaabyServer server, params Type[] types)
        {
            LoadHelper.FromAssemblyOf(types);
            return server;
        }

        public static ZaabyServer FromAssemblies(this ZaabyServer server, params Assembly[] assemblies)
        {
            LoadHelper.FromAssemblies(assemblies);
            return server;
        }

        public static ZaabyServer FromAssemblyNames(this ZaabyServer server, params AssemblyName[] assemblyNames)
        {
            LoadHelper.FromAssemblyNames(assemblyNames);
            return server;
        }

        public static ZaabyServer FromDirectories(this ZaabyServer server, params string[] directories)
        {
            LoadHelper.FromDirectories(directories);
            return server;
        }
    }
}