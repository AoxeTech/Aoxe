namespace Aoxe.Extensions.Configuration.Consul;

public class ConsulConfigurationProvider : ConfigurationProvider, IDisposable
{
    private readonly ConsulClient _consulClient;
    private readonly string _key;
    private readonly IParser _parser;

    public ConsulConfigurationProvider(ConsulConfigurationOptions options)
    {
        _key = options.Key.Trim();
        _consulClient = new ConsulClient(
            options.ConfigOverride,
            options.ClientOverride,
            options.HandlerOverride
        );
        _parser = options.Parser;
    }

    public override void Load()
    {
        if (string.IsNullOrWhiteSpace(_key))
        {
            var queryResult = _consulClient.KV.List(_folder).Result;
            if (queryResult?.StatusCode is not HttpStatusCode.OK || queryResult.Response is null)
                return;
            foreach (var item in queryResult.Response.Where(p => p.Value is not null))
            foreach (var kv in GetKeyValuePair(item.Key, item.Value))
                Set(kv.Key, kv.Value);
        }
        else
        {
            var folder = _folder ?? "/";
            var queryResult = _consulClient.KV.Get($"{folder}/{_key}").Result;
            if (
                queryResult?.StatusCode is not HttpStatusCode.OK
                || queryResult.Response?.Value is null
            )
                return;
            foreach (
                var kv in GetKeyValuePair(queryResult.Response.Key, queryResult.Response.Value)
            )
                Set(kv.Key, kv.Value);
        }
    }

    private Dictionary<string, string> GetKeyValuePair(string nodeName, byte[] bytes) =>
        _parser.Parse(nodeName, bytes);

    public void Dispose()
    {
        _consulClient.Dispose();
    }
}
