using System;
using Consul;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Zaaby.Server.Consul
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddServiceRegistry(this IServiceCollection services,
            Action<ZaabeeConsulOptions> optionsFactory)
        {
            var options = new ZaabeeConsulOptions();
            optionsFactory(options);
            services.Configure(optionsFactory);
            services.TryAddSingleton<IConsulClient>(_ => new ConsulClient(x =>
            {
                x.Address = new Uri(options.ConsulAddress);
            }));
            services.AddHostedService<ServiceRegistry>();
            return services;
        }
    }
}