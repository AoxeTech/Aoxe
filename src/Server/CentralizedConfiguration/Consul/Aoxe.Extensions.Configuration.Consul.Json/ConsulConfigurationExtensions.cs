namespace Aoxe.Extensions.Configuration.Consul.Json;

public static class ConsulConfigurationExtensions
{
    public static IConfigurationBuilder AddConsulJson(
        this IConfigurationBuilder builder,
        ConsulClientConfiguration consulClientConfiguration,
        string key
    ) =>
        builder.Add(
            new ConsulConfigurationSource(consulClientConfiguration, key, new JsonFlattener())
        );
}
