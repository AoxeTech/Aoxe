using System;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using Zaaby.Common;

namespace Zaaby.Server
{
    public static partial class ZaabyIServiceCollectionExtensions
    {
        public static IServiceCollection AddZaabyService<TService>(this IServiceCollection services) =>
            services.AddZaabyService(typeof(TService));

        public static IServiceCollection AddZaabyService(this IServiceCollection services, Type serviceDefineType)
        {
            var typePairs = typeof(Attribute).IsAssignableFrom(serviceDefineType)
                ? LoadHelper.GetByAttribute(serviceDefineType)
                : LoadHelper.GetByBaseType(serviceDefineType);

            services.AddControllers(options =>
                {
                    foreach (var type in typePairs.Where(t => t.ImplementationType is not null)
                        .Select(t => t.InterfaceType ?? t.ImplementationType))
                        options.Conventions.Add(new ZaabyActionModelConvention(type));
                })
                .ConfigureApplicationPartManager(manager =>
                {
                    manager.FeatureProviders.Add(
                        new ZaabyAppServiceControllerFeatureProvider(typePairs
                            .Where(t => t.ImplementationType is not null)
                            .Select(t => t.ImplementationType)
                            .ToList()));
                });
            return services;
        }
    }
}