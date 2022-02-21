using System.Net.Http;
using dotnet_etcd;
using Microsoft.Extensions.Configuration;

namespace Zaaby.Extensions.Configuration.Etcd
{
    public class EtcdConfigurationSource : IConfigurationSource
    {
        private readonly EtcdClient _etcdClient;

        public EtcdConfigurationSource(
            string connectionString,
            int port = 2379,
            HttpClientHandler handler = null,
            bool ssl = false,
            bool useLegacyRpcExceptionForCancellation = false)
        {
            _etcdClient = new EtcdClient(connectionString, port, handler, ssl, useLegacyRpcExceptionForCancellation);
        }

        public IConfigurationProvider Build(IConfigurationBuilder builder) =>
            new EtcdConfigurationProvider(_etcdClient);
    }
}