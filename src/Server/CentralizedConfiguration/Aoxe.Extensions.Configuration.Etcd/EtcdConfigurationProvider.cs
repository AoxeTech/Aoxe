namespace Aoxe.Extensions.Configuration.Etcd;

public class EtcdConfigurationProvider(EtcdClient etcdClient) : ConfigurationProvider
{
    private readonly EtcdClient _etcdClient = etcdClient;

    public override void Load() { }
}
