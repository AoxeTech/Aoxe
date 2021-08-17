using System;
using Consul;
using Microsoft.Extensions.DependencyInjection;

namespace Zaaby.Consul
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddConsul(this IServiceCollection services,
            Action<ZaabeeConsulOptions> optionsFactory)
        {
            var options = new ZaabeeConsulOptions();
            optionsFactory(options);
            services.Configure(optionsFactory);
            services.AddSingleton<IConsulClient>(_ => new ConsulClient(x =>
            {
                x.Address = new Uri(options.ConsulAddress);
            }));
            //注册自定义的DelegatingHandler
            services.AddTransient<ConsulDiscoveryDelegatingHandler>();
            services.AddHostedService<ServiceRegistry>();
            return services;
        }
    }
}