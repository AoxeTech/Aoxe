namespace Aoxe.Extensions.Configuration.Etcd;

public class EtcdConfigurationProvider(EtcdClient etcdClient, string key) : ConfigurationProvider
{
    public override void Load()
    {
        var data = etcdClient.GetRangeVal(key);
        Data = data;
    }
}
