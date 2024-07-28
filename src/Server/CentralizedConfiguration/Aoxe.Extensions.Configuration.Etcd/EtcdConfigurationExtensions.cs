namespace Aoxe.Extensions.Configuration.Etcd;

public static class EtcdConfigurationExtensions
{
    public static IConfigurationBuilder AddEtcd(
        this IConfigurationBuilder builder,
        EtcdClient etcdClient,
        string key
    )
    {
        return builder.Add(new EtcdConfigurationSource(etcdClient, key));
    }
}
