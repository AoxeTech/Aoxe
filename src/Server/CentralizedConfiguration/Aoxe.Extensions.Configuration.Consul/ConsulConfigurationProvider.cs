namespace Aoxe.Extensions.Configuration.Consul;

public class ConsulConfigurationProvider(ConsulClient client, string key) : ConfigurationProvider
{
    public override void Load()
    {
        var result = client.KV.Get(key).Result;
        if (result.Response is null)
            return;
        var data = Encoding.UTF8.GetString(result.Response.Value);
        var dict = new Dictionary<string, string?> { { key, data } };
        Data = dict;
    }
}
