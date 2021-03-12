using System;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using Zaaby.Common;

namespace Zaaby.Service
{
    public static class ZaabyIServiceCollectionExtensions
    {
        public static IServiceCollection AddZaaby<TService>(this IServiceCollection services) =>
            AddZaaby(services, type =>
                type.IsInterface && typeof(TService).IsAssignableFrom(type) && type != typeof(TService));

        public static IServiceCollection AddZaaby(this IServiceCollection services, Func<Type, bool> definition)
        {
            if (!LoadHelper.Types.Any()) LoadHelper.LoadAllTypes();
            var allTypes = LoadHelper.Types;
            var interfaceTypes = allTypes.Where(definition).ToList();

            var implementTypes =
                allTypes.Where(type => type.IsClass && interfaceTypes.Any(i => i.IsAssignableFrom(type)));

            services.AddControllers(options => interfaceTypes.ForEach(interfaceType =>
                    options.Conventions.Add(new ZaabyActionModelConvention(interfaceType))))
                .ConfigureApplicationPartManager(manager =>
                    manager.FeatureProviders.Add(new ZaabyAppServiceControllerFeatureProvider(implementTypes)));
            return services;
        }
    }
}