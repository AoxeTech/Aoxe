using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;

namespace Zaaby.Common
{
    public static partial class ZaabyIServiceCollectionExtensions
    {
        public static IServiceCollection Register<TDefineType>(this IServiceCollection services,
            ServiceLifetime serviceLifetime) =>
            services.Register(typeof(TDefineType), serviceLifetime);

        public static IServiceCollection Register(this IServiceCollection services, Type defineType,
            ServiceLifetime serviceLifetime)
        {
            var typePairs = LoadHelper.GetTypePairs(defineType);
            return services.Register(typePairs, serviceLifetime);
        }

        private static IServiceCollection Register(this IServiceCollection services,
            IEnumerable<TypePair> typePairs, ServiceLifetime serviceLifetime)
        {
            var serviceDescriptors = typePairs.Where(tp => tp.ImplementationType is not null)
                .Select(tp => new ServiceDescriptor(tp.InterfaceType ?? tp.ImplementationType,
                    tp.ImplementationType,
                    serviceLifetime));
            foreach (var serviceDescriptor in serviceDescriptors) services.Add(serviceDescriptor);
            return services;
        }
    }
}