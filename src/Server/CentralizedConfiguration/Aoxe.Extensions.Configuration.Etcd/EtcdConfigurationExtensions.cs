namespace Aoxe.Extensions.Configuration.Etcd;

public static class EtcdConfigurationExtensions
{
    public static IConfigurationBuilder AddEtcd(
        this IConfigurationBuilder builder,
        string connectionString,
        int port = 2379,
        string serverName = "my-etcd-server",
        Action<GrpcChannelOptions>? configureChannelOptions = null,
        Interceptor[]? interceptors = null
    )
    {
        builder.Add(
            new EtcdConfigurationSource(
                connectionString,
                port,
                serverName,
                configureChannelOptions,
                interceptors
            )
        );
        return builder;
    }
}
