namespace Aoxe.Extensions.Configuration.Consul;

public class ConsulJsonConfigurationProvider(ConsulConfigurationOptions options)
    : ConfigurationProvider,
        IDisposable
{
    private readonly ConsulClient _consulClient =
        new(options.ConfigOverride, options.ClientOverride, options.HandlerOverride);
    private readonly string? _key = options.Key.Trim();

    public override void Load()
    {
        if (string.IsNullOrWhiteSpace(_key))
        {
            var queryResult = _consulClient.KV.List(_key).GetAwaiter().GetResult();
            if (queryResult?.StatusCode is not HttpStatusCode.OK || queryResult.Response is null)
                return;
            foreach (var item in queryResult.Response)
                SetKvPair(item.Value);
        }
        else
        {
            var queryResult = _consulClient.KV.Get(_key).GetAwaiter().GetResult();
            if (
                queryResult?.StatusCode is not HttpStatusCode.OK
                || queryResult.Response?.Value is null
            )
                return;
            SetKvPair(queryResult.Response.Value);
        }
    }

    private void SetKvPair(byte[] bytes)
    {
        var json = Encoding.UTF8.GetString(bytes);
        using var memoryStream = new MemoryStream();
        using var streamWriter = new StreamWriter(memoryStream);
        streamWriter.Write(json);
        streamWriter.Flush();
        memoryStream.Position = 0;
        var parser = new JsonConfigurationObjectParser();
        foreach (var keyValuePair in parser.Parse(memoryStream))
            Set(keyValuePair.Key, keyValuePair.Value);
    }

    public void Dispose()
    {
        _consulClient.Dispose();
    }
}
