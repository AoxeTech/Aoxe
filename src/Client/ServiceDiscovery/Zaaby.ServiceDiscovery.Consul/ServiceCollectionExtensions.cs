using System;
using System.Linq;
using Consul;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Zaaby.Client.Http;
using Zaaby.Shared;

namespace Zaaby.ServiceDiscovery.Consul;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddServiceDiscovery(this IServiceCollection services, Type serviceDefineType,
        string consulAddress)
    {
        services.TryAddSingleton<IConsulClient>(_ => new ConsulClient(x =>
        {
            x.Address = new Uri(consulAddress);
        }));
        services.AddTransient<ConsulServiceDiscoveryDelegatingHandler>();

        var consulClient = services.BuildServiceProvider().GetService<IConsulClient>();
        var serviceNames = consulClient?.Catalog.Services().Result.Response;

        var i = LoadHelper.GetTypePairs(serviceDefineType)
            .Where(p => p.InterfaceType?.Namespace is not null && p.ImplementationType is null);

        // foreach (var typeWithUri in typeWitUris)
        // {
        //     services.AddHttpClient(typeWithUri.Type.Namespace)
        //         .AddHttpMessageHandler<ConsulServiceDiscoveryDelegatingHandler>();
        //     services.AddScoped(typeWithUri.Type, _ => methodInfo.MakeGenericMethod(typeWithUri.Type)
        //         .Invoke(services.BuildServiceProvider().GetService<ZaabyClient>(), null));
        // }

        return services;
    }
}