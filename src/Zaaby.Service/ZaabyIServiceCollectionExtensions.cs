using System;
using Microsoft.Extensions.DependencyInjection;
using Zaaby.Common;

namespace Zaaby.Service
{
    public static class ZaabyIServiceCollectionExtensions
    {
        public static IServiceCollection AddZaaby<TService>(this IServiceCollection services) =>
            services.AddZaaby(typeof(TService));

        public static IServiceCollection AddZaaby(this IServiceCollection services, Type baseType)
        {
            var (interfaceTypes, classTypes, _, allInterfacesNotAssignClassTypes) =
                LoadHelper.GetByBaseType(baseType);

            services.AddControllers(options =>
                {
                    interfaceTypes.ForEach(type =>
                        options.Conventions.Add(new ZaabyActionModelConvention(type)));
                    allInterfacesNotAssignClassTypes.ForEach(type =>
                        options.Conventions.Add(new ZaabyActionModelConvention(type)));
                })
                .ConfigureApplicationPartManager(manager =>
                {
                    // manager.FeatureProviders.Add(new ZaabyAppServiceControllerFeatureProvider(implementTypes));
                    manager.FeatureProviders.Add(new ZaabyAppServiceControllerFeatureProvider(classTypes));
                });
            return services;
        }
    }
}