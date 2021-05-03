using System;
using Microsoft.Extensions.DependencyInjection;

namespace Zaaby.Common
{
    public static class ZaabyIServiceCollectionExtensions
    {
        public static IServiceCollection FromAssemblyOf<T>(this IServiceCollection services)
        {
            LoadHelper.FromAssemblyOf<T>();
            return services;
        }

        public static IServiceCollection FromAssemblyOf(this IServiceCollection services, Type type)
        {
            LoadHelper.FromAssemblyOf(type);
            return services;
        }
    }
}