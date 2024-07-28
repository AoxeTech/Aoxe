namespace Aoxe.Extensions.Configuration.Etcd;

public class EtcdConfigurationSource(EtcdClient etcdClient, string key) : IConfigurationSource
{
    public IConfigurationProvider Build(IConfigurationBuilder builder)
    {
        return new EtcdConfigurationProvider(etcdClient, key);
    }
}
