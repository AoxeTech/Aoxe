namespace Aoxe.Extensions.Configuration.Consul.Xml;

public static class ConsulConfigurationExtensions
{
    public static IConfigurationBuilder AddConsulXml(
        this IConfigurationBuilder builder,
        ConsulClientConfiguration consulClientConfiguration,
        string key
    ) =>
        builder.Add(
            new ConsulConfigurationSource(consulClientConfiguration, key, new XmlFlattener())
        );
}
