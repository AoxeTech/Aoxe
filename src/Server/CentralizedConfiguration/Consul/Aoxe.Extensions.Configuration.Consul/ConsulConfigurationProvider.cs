namespace Aoxe.Extensions.Configuration.Consul;

public class ConsulConfigurationProvider(ConsulConfigurationSource source, IFlattener? flattener)
    : ConfigurationProvider,
        IDisposable
{
    private readonly ConsulClient _consulClient = new(source.ConsulClientConfiguration);

    public override void Load()
    {
        var result = _consulClient.KV.Get(source.Key).Result;
        if (result.Response is null)
            return;
        Data = flattener is null
            ? new Dictionary<string, string?>
            {
                { source.Key, Encoding.UTF8.GetString(result.Response.Value) }
            }
            : flattener.Flatten(new MemoryStream(result.Response.Value));
    }

    public void Dispose()
    {
        _consulClient.Dispose();
    }
}
