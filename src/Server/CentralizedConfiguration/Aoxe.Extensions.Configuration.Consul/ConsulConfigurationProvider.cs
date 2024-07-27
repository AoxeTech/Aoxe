namespace Aoxe.Extensions.Configuration.Consul;

public class ConsulConfigurationProvider(ConsulConfigurationOptions options)
    : ConfigurationProvider,
        IDisposable
{
    private readonly ConsulClient _consulClient =
        new(options.ConfigOverride, options.ClientOverride, options.HandlerOverride);
    private readonly string _key = options.Key.Trim();
    private readonly IParser _parser = options.Parser;

    public override void Load()
    {
        if (string.IsNullOrWhiteSpace(_key))
        {
            var queryResult = _consulClient.KV.List(_key).Result;
            if (queryResult?.StatusCode is not HttpStatusCode.OK || queryResult.Response is null)
                return;
            foreach (var item in queryResult.Response.Where(p => p.Value is not null))
            foreach (var kv in GetKeyValuePair(item.Key, item.Value))
                Set(kv.Key, kv.Value);
        }
        else
        {
            var queryResult = _consulClient.KV.Get(_key).Result;
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
