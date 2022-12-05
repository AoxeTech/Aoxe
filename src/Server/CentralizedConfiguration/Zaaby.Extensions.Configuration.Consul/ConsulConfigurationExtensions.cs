namespace Zaaby.Extensions.Configuration.Consul;

public static class ConsulConfigurationExtensions
{
    public static IConfigurationBuilder AddConsul(
            this IConfigurationBuilder builder,
            Action<ConsulConfigurationOptions> optionsFactory)
    {
        builder
            .AddInMemoryCollection()
            .AddIniFile()
            .AddIniStream()
            .AddJsonFile()
            .AddJsonStream()
            .AddXmlFile()
            .AddXmlStream()
            .AddKeyPerFile()
            .AddAzureKeyVault();
        var options = new ConsulConfigurationOptions();
        optionsFactory(options);
        builder.Add(new ConsulConfigurationSource(options));
        return builder;
    }
}