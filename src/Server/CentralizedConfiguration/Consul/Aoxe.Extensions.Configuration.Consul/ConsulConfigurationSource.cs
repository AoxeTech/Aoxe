namespace Aoxe.Extensions.Configuration.Consul;

public class ConsulConfigurationSource(
    ConsulClientConfiguration consulClientConfiguration,
    string key,
    IFlattener? flattener = null
) : IConfigurationSource
{
    public ConsulClientConfiguration ConsulClientConfiguration { get; } = consulClientConfiguration;
    public string Key { get; } = key;

    public IConfigurationProvider Build(IConfigurationBuilder builder) =>
        new ConsulConfigurationProvider(this, flattener);
}
