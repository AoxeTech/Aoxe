namespace Zaaby.Extensions.Configuration.Consul;

public class ConsulConfigurationSource : IConfigurationSource
{
    private readonly Action<ConsulClientConfiguration>? _configOverride;
    private readonly Action<HttpClient>? _clientOverride;
    private readonly Action<HttpClientHandler>? _handlerOverride;

    private readonly string? _folder;
    private readonly string? _key;

    public ConsulConfigurationSource(Action<ConsulClientConfiguration>? configOverride = null,
        Action<HttpClient>? clientOverride = null,
        Action<HttpClientHandler>? handlerOverride = null,
        string? folder = "/",
        string? key = null)
    {
        _folder = folder;
        _key = key;
        _configOverride = configOverride;
        _clientOverride = clientOverride;
        _handlerOverride = handlerOverride;
    }

    public IConfigurationProvider Build(IConfigurationBuilder builder) =>
        new ConsulConfigurationProvider(new ConsulClient(_configOverride, _clientOverride, _handlerOverride),
            _folder,
            _key);
}