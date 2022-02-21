namespace Zaaby.Extensions.Configuration.Consul;

public class ConsulConfigurationProvider : ConfigurationProvider
{
    private readonly ConsulClient _consulClient;
    private readonly string? _folder;
    private readonly string? _key;

    public ConsulConfigurationProvider(ConsulClient consulClient, string? folder = "/", string? key = null)
    {
        _folder = folder?.Trim();
        _key = key?.Trim();
        _consulClient = consulClient;
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
                SetKvPair(item.Value);
        }
        else
        {
            var queryResult = _consulClient.KV.Get($"{folder}/{_key}").Result;
            if (queryResult?.StatusCode is not HttpStatusCode.OK || queryResult.Response?.Value is null)
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
}