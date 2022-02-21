using dotnet_etcd;
using Microsoft.Extensions.Configuration;

namespace Zaaby.Extensions.Configuration.Etcd
{
    public class EtcdConfigurationProvider : ConfigurationProvider
    {
        private readonly EtcdClient _etcdClient;

        public EtcdConfigurationProvider(EtcdClient etcdClient)
        {
            _etcdClient = etcdClient;
        }

        public override void Load()
        {
            
        }
    }
}