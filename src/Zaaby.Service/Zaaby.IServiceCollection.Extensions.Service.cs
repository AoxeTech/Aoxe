using System;
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
            var types = LoadHelper.GetByBaseType(baseServiceType);

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