namespace Aoxe.Extensions.Configuration.Etcd;

public class EtcdConfigurationSource(
    string connectionString,
    int port = 2379,
    string serverName = "my-etcd-server",
    Action<GrpcChannelOptions>? configureChannelOptions = null,
    Interceptor[]? interceptors = null
) : IConfigurationSource
{
    private readonly EtcdClient _etcdClient =
        new(connectionString, port, serverName, configureChannelOptions, interceptors);

    public IConfigurationProvider Build(IConfigurationBuilder builder) =>
        new EtcdConfigurationProvider(_etcdClient);
}
