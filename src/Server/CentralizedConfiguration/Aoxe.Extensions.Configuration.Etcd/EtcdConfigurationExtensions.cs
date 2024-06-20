using System.Net.Http;
using Grpc.Core.Interceptors;
using Grpc.Net.Client.Configuration;
using Microsoft.Extensions.Configuration;

namespace Aoxe.Extensions.Configuration.Etcd
{
    public static class EtcdConfigurationExtensions
    {
        public static IConfigurationBuilder AddEtcd(
            this IConfigurationBuilder builder,
            string connectionString,
            int port = 2379,
            string serverName = "my-etcd-server",
            HttpClientHandler? handler = null,
            bool ssl = false,
            bool useLegacyRpcExceptionForCancellation = false,
            Interceptor[]? interceptors = null,
            MethodConfig? grpcMethodConfig = null,
            RetryThrottlingPolicy? grpcRetryThrottlingPolicy = null
        )
        {
            builder.Add(
                new EtcdConfigurationSource(
                    connectionString,
                    port,
                    serverName,
                    handler,
                    ssl,
                    useLegacyRpcExceptionForCancellation,
                    interceptors,
                    grpcMethodConfig,
                    grpcRetryThrottlingPolicy
                )
            );
            return builder;
        }
    }
}
