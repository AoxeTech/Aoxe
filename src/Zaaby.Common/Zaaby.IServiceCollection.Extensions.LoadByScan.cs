using System;
using Microsoft.Extensions.DependencyInjection;

namespace Zaaby.Common
{
    public static partial class ZaabyIServiceCollectionExtensions
    {
        public static IServiceCollection FromAssemblyOf<T>(this IServiceCollection services)
        {
            LoadHelper.FromAssemblyOf<T>();
            return services;
        }

        public static IServiceCollection FromAssemblyOf<T0, T1>(this IServiceCollection services)
        {
            LoadHelper.FromAssemblyOf<T0, T1>();
            return services;
        }

        public static IServiceCollection FromAssemblyOf<T0, T1, T2>(this IServiceCollection services)
        {
            LoadHelper.FromAssemblyOf<T0, T1, T2>();
            return services;
        }

        public static IServiceCollection FromAssemblyOf<T0, T1, T2, T3>(this IServiceCollection services)
        {
            LoadHelper.FromAssemblyOf<T0, T1, T2, T3>();
            return services;
        }

        public static IServiceCollection FromAssemblyOf<T0, T1, T2, T3, T4>(this IServiceCollection services)
        {
            LoadHelper.FromAssemblyOf<T0, T1, T2, T3, T4>();
            return services;
        }

        public static IServiceCollection FromAssemblyOf<T0, T1, T2, T3, T4, T5>(this IServiceCollection services)
        {
            LoadHelper.FromAssemblyOf<T0, T1, T2, T3, T4, T5>();
            return services;
        }

        public static IServiceCollection FromAssemblyOf<T0, T1, T2, T3, T4, T5, T6>(this IServiceCollection services)
        {
            LoadHelper.FromAssemblyOf<T0, T1, T2, T3, T4, T5, T6>();
            return services;
        }

        public static IServiceCollection FromAssemblyOf<T0, T1, T2, T3, T4, T5, T6, T7>(
            this IServiceCollection services)
        {
            LoadHelper.FromAssemblyOf<T0, T1, T2, T3, T4, T5, T6, T7>();
            return services;
        }

        public static IServiceCollection FromAssemblyOf<T0, T1, T2, T3, T4, T5, T6, T7, T8>(
            this IServiceCollection services)
        {
            LoadHelper.FromAssemblyOf<T0, T1, T2, T3, T4, T5, T6, T7, T8>();
            return services;
        }

        public static IServiceCollection FromAssemblyOf<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9>(
            this IServiceCollection services)
        {
            LoadHelper.FromAssemblyOf<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9>();
            return services;
        }

        public static IServiceCollection FromAssemblyOf(this IServiceCollection services, params Type[] types)
        {
            LoadHelper.FromAssemblyOf(types);
            return services;
        }
    }
}