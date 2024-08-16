namespace Aoxe.Extensions.Configuration.Consul;

public static class ConsulConfigurationExtensions
{
    public static IConfigurationBuilder AddConsul(
        this IConfigurationBuilder builder,
        ConsulClientConfiguration consulClientConfiguration,
        string key,
        IFlattener? flattener = null
    ) => builder.Add(new ConsulConfigurationSource(consulClientConfiguration, key, flattener));
}
