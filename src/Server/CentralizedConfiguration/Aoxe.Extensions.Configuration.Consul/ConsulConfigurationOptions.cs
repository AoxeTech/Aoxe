namespace Aoxe.Extensions.Configuration.Consul;

public class ConsulConfigurationOptions
{
    public Action<ConsulClientConfiguration>? ConfigOverride { get; set; }
    public Action<HttpClient>? ClientOverride { get; set; }
    public Action<HttpClientHandler>? HandlerOverride { get; set; }
    public string Key { get; set; } = string.Empty;
    public IParser Parser { get; set; } = new SimpleParser();
}
