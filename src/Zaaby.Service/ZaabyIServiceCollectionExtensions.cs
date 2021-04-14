using System;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using Zaaby.Common;

namespace Zaaby.Service
{
    public static class ZaabyIServiceCollectionExtensions
    {
        public static IServiceCollection AddZaaby<TService>(this IServiceCollection services) =>
            AddZaaby(services, type => typeof(TService).IsAssignableFrom(type) && type != typeof(TService));

        public static IServiceCollection AddZaaby(this IServiceCollection services, Func<Type, bool> definition)
        {
            if (!LoadHelper.Types.Any()) LoadHelper.LoadAllTypes();
            var types = LoadHelper.Types.Where(definition).ToList();

            var interfaceTypes = types.Where(type => type.IsInterface).ToList();
            var serviceTypes = types.Where(type => type.IsClass).ToList();
            var notAssignableFromTypes =
                serviceTypes.Where(type => !interfaceTypes.Any(i => i.IsAssignableFrom(type))).ToList();

            services.AddControllers(options =>
                {
                    interfaceTypes.ForEach(interfaceType =>
                        options.Conventions.Add(new ZaabyActionModelConvention(interfaceType)));
                    notAssignableFromTypes.ForEach(notAssignableFromType=>
                        options.Conventions.Add(new ZaabyActionModelConvention(notAssignableFromType)));
                })
                .ConfigureApplicationPartManager(manager =>
                {
                    // manager.FeatureProviders.Add(new ZaabyAppServiceControllerFeatureProvider(implementTypes));
                    manager.FeatureProviders.Add(new ZaabyAppServiceControllerFeatureProvider(serviceTypes));
                });
            return services;
        }
    }
}