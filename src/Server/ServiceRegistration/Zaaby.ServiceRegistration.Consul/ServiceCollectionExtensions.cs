namespace Zaaby.ServiceRegistration.Consul;

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
            x.Address = options.ConsulAddress;
            x.Datacenter = options.Datacenter;
            x.Token = options.Token;
            x.WaitTime = options.WaitTime;
        }));
        services.AddHostedService<ServiceRegistry>();
        return services;
    }
}