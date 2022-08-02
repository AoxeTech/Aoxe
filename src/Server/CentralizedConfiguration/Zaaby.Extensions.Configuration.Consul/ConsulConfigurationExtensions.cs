namespace Zaaby.Extensions.Configuration.Consul;

public static class ConsulConfigurationExtensions
{
    public static IConfigurationBuilder AddConsul(this IConfigurationBuilder builder,
        Action<ConsulClientConfiguration>? configOverride = null,
        Action<HttpClient>? clientOverride = null,
        Action<HttpClientHandler>? handlerOverride = null,
        string folder = "/",
        string? key = null)
    {
        builder.Add(new ConsulConfigurationSource(configOverride, clientOverride, handlerOverride, folder, key));
        return builder;
    }
}