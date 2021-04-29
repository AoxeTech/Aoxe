using System;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using Zaaby.Common;

namespace Zaaby.Service
{
    public static partial class ZaabyIServiceCollectionExtensions
    {
        public static IServiceCollection AddZaabyService<TService>(this IServiceCollection services) =>
            services.AddZaabyService(typeof(TService));

        public static IServiceCollection AddZaabyService(this IServiceCollection services, Type baseServiceType)
        {
            var typePairs = LoadHelper.GetByBaseType(baseServiceType);

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