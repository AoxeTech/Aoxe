namespace Zaaby.Extensions.Configuration.Consul;

public class ConsulConfigurationSource : IConfigurationSource
{
    private readonly ConsulConfigurationOptions _consulConfigurationOptions;

    public ConsulConfigurationSource(ConsulConfigurationOptions options)
    {
        _consulConfigurationOptions = options;
    }

    public IConfigurationProvider Build(IConfigurationBuilder builder) =>
        new ConsulConfigurationProvider(_consulConfigurationOptions);
}