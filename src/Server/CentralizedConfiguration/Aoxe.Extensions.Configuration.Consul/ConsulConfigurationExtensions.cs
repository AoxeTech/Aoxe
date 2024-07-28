namespace Aoxe.Extensions.Configuration.Consul;

public static class ConsulConfigurationExtensions
{
    public static IConfigurationBuilder AddConsul(
        this IConfigurationBuilder builder,
        ConsulClient client,
        string key
    )
    {
        return builder.Add(new ConsulConfigurationSource(client, key));
    }
}
