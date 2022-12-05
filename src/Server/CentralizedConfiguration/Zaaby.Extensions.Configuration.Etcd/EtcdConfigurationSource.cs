using System.Net.Http;
using dotnet_etcd;
using Grpc.Core.Interceptors;
using Grpc.Net.Client.Configuration;
using Microsoft.Extensions.Configuration;

namespace Zaaby.Extensions.Configuration.Etcd
{
    public class EtcdConfigurationSource : IConfigurationSource
    {
        private readonly EtcdClient _etcdClient;

        public EtcdConfigurationSource(
            string connectionString,
            int port = 2379,
            string serverName = "my-etcd-server",
            HttpClientHandler? handler = null,
            bool ssl = false,
            bool useLegacyRpcExceptionForCancellation = false,
            Interceptor[]? interceptors = null,
            MethodConfig? grpcMethodConfig = null,
            RetryThrottlingPolicy? grpcRetryThrottlingPolicy = null)
        {
            _etcdClient = new EtcdClient(
                connectionString,
                port, serverName,
                handler,
                ssl,
                useLegacyRpcExceptionForCancellation,
                interceptors,
                grpcMethodConfig,
                grpcRetryThrottlingPolicy);
        }

        public IConfigurationProvider Build(IConfigurationBuilder builder) =>
            new EtcdConfigurationProvider(_etcdClient);
    }
}