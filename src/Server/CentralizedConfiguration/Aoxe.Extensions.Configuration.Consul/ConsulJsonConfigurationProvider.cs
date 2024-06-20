namespace Aoxe.Extensions.Configuration.Consul;

public class ConsulJsonConfigurationProvider : ConfigurationProvider, IDisposable
{
    private readonly ConsulClient _consulClient;
    private readonly string? _folder;
    private readonly string? _key;

    public ConsulJsonConfigurationProvider(ConsulConfigurationOptions options)
    {
        _folder = options.Folder?.Trim();
        _key = options.Key?.Trim();
        _consulClient = new ConsulClient(
            options.ConfigOverride,
            options.ClientOverride,
            options.HandlerOverride
        );
    }

    public override void Load()
    {
        var folder = _folder ?? "/";
        if (string.IsNullOrWhiteSpace(_key))
        {
            var queryResult = _consulClient.KV.List(_folder).GetAwaiter().GetResult();
            if (queryResult?.StatusCode is not HttpStatusCode.OK || queryResult.Response is null)
                return;
            foreach (var item in queryResult.Response)
                SetKvPair(item.Value);
        }
        else
        {
            var queryResult = _consulClient.KV.Get($"{folder}/{_key}").GetAwaiter().GetResult();
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
        using (var memoryStream = new MemoryStream())
        using (var streamWriter = new StreamWriter(memoryStream))
        {
            streamWriter.Write(json);
            streamWriter.Flush();
            memoryStream.Position = 0;
            var parser = new JsonConfigurationObjectParser();
            foreach (var keyValuePair in parser.Parse(memoryStream))
                Set(keyValuePair.Key, keyValuePair.Value);
        }
    }

    public void Dispose()
    {
        _consulClient.Dispose();
    }
}
