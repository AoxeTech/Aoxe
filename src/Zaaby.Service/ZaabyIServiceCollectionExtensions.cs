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
            var types = LoadHelper.GetByBaseType(baseType);

            services.AddControllers(options =>
                {
                    types.InterfaceTypes.ForEach(type =>
                        options.Conventions.Add(new ZaabyActionModelConvention(type)));
                    types.AllInterfacesNotAssignClassTypes.ForEach(type =>
                        options.Conventions.Add(new ZaabyActionModelConvention(type)));
                })
                .ConfigureApplicationPartManager(manager =>
                {
                    // manager.FeatureProviders.Add(new ZaabyAppServiceControllerFeatureProvider(implementTypes));
                    manager.FeatureProviders.Add(new ZaabyAppServiceControllerFeatureProvider(types.ClassTypes));
                });
            return services;
        }
    }
}