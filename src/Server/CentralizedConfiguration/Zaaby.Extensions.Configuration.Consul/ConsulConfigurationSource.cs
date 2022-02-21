namespace Zaaby.Extensions.Configuration.Consul;

public class ConsulConfigurationSource : IConfigurationSource
{
    private readonly ConsulClient _consulClient;
    private readonly string? _folder;
    private readonly string? _key;

    public ConsulConfigurationSource(Action<ConsulClientConfiguration>? configOverride = null,
        Action<HttpClient>? clientOverride = null, Action<HttpClientHandler>? handlerOverride = null,
        string? folder = "/", string? key = null)
    {
        _folder = folder;
        _key = key;
        _consulClient = new ConsulClient(configOverride, clientOverride, handlerOverride);
    }

    public IConfigurationProvider Build(IConfigurationBuilder builder) =>
        new ConsulConfigurationProvider(_consulClient, _folder, _key);
}