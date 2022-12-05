namespace Zaaby.Extensions.Configuration.Consul;

public class ConsulJsonConfigurationProvider : ConfigurationProvider, IDisposable
{
    private readonly ConsulClient _consulClient;
    private readonly string? _folder;
    private readonly string? _key;
    private readonly IParser _parser;

    public ConsulJsonConfigurationProvider(ConsulConfigurationOptions options)
    {
        _folder = options.Folder?.Trim();
        _key = options.Key?.Trim();
        _consulClient = new ConsulClient(options.ConfigOverride, options.ClientOverride, options.HandlerOverride);
        _parser = options.Parser;
    }

    public override void Load()
    {
        var folder = _folder ?? "/";
        if (string.IsNullOrWhiteSpace(_key))
        {
            var queryResult = _consulClient.KV.List(_folder).Result;
            if (queryResult?.StatusCode is not HttpStatusCode.OK || queryResult.Response is null)
                return;
            foreach (var item in queryResult.Response.Where(p => p.Value is not null))
                _parser.Load();
        }
        else
        {
            var queryResult = _consulClient.KV.Get($"{folder}/{_key}").Result;
            if (queryResult?.StatusCode is not HttpStatusCode.OK || queryResult.Response?.Value is null)
                return;
            _parser.Load();
        }
    }

    public void Dispose()
    {
        _consulClient.Dispose();
    }
}