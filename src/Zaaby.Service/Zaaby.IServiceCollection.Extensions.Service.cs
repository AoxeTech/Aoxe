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
                    foreach (var type in typePairs.Select(typePair =>
                        typePair.InterfaceType ?? typePair.ImplementationType))
                        options.Conventions.Add(new ZaabyActionModelConvention(type));
                })
                .ConfigureApplicationPartManager(manager =>
                {
                    manager.FeatureProviders.Add(
                        new ZaabyAppServiceControllerFeatureProvider(typePairs.Select(t => t.ImplementationType)
                            .ToList()));
                });
            return services;
        }
    }
}