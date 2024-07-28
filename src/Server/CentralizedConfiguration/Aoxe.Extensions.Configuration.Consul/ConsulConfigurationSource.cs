namespace Aoxe.Extensions.Configuration.Consul;

public class ConsulConfigurationSource(ConsulClient client, string key) : IConfigurationSource
{
    public IConfigurationProvider Build(IConfigurationBuilder builder)
    {
        return new ConsulConfigurationProvider(client, key);
    }
}
